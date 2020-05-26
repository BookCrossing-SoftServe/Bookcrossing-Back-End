using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Application.Dto.QueryParams;
using Application.Dto.QueryParams.Enums;

namespace Application.QueryableExtension
{
    public static class QueryableExtension
    {
        private static readonly Type StringType = typeof(string);
        private static readonly Type QueryableType = typeof(Queryable);
        private static readonly MethodInfo ContainsMethod = StringType.GetMethod("Contains", new[] {StringType});

        #region Where

        public static IQueryable<T> Where<T>(this IQueryable<T> query, params FilterParameters[] filters)
        {
            if (filters == null || filters.Length < 1)
            {
                return query;
            }

            Expression method = null;
            var param = Expression.Parameter(typeof(T), "x");
            for (var index = 0; index < filters.Length; index++)
            {
                Expression propertyExp = param;
                var type = typeof(T);

                var split = filters[index].PropertyName.Split('.');
                foreach (var t in split)
                {
                    PropertyInfo pi = GetProperty(type, t);
                    propertyExp = Expression.Property(propertyExp, pi);
                    type = pi.PropertyType;
                }

                try
                {
                    method = method == null
                        ? GetMethodExpression(propertyExp, filters[index])
                        : GetOperandExpression(method, GetMethodExpression(propertyExp, filters[index]),
                            filters[index - 1]);
                }
                catch (ArgumentException)
                {
                    throw new InvalidOperationException("Value doesn't match the method's type");
                }
            }

            if (method == null)
                throw new InvalidOperationException("Failed to create method expression");

            var lambda = Expression.Lambda<Func<T, bool>>(method, param);
            return query.Where(lambda);
        }

        private static Expression GetMethodExpression(Expression property, FilterParameters filter)
        {
            switch (filter.Method)
            {
                case FilterMethod.Contains:
                    return Expression.Call(property, ContainsMethod, Expression.Constant(filter.Value, StringType));
                case FilterMethod.Equal:
                    return Expression.Equal(property,
                        GetConstant(TryCastValueType(filter.Value, property.Type), property.Type));
                case FilterMethod.NotEqual:
                    return Expression.NotEqual(property,
                        GetConstant(TryCastValueType(filter.Value, property.Type), property.Type));
                default:
                    return property;
            }
        }

        private static Expression GetOperandExpression(Expression left, Expression right, FilterParameters filter)
        {
            switch (filter.Operand)
            {
                case FilterOperand.And:
                    return Expression.AndAlso(left, right);
                default:
                    return Expression.OrElse(left, right);
            }
        }

        private static object TryCastValueType(object value, Type type)
        {
            var valueType = value.GetType();
            if (valueType == type)
                return value;


            var s = Convert.ToString(value);

            object res;

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
                res = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type));
            }
            else
            {
                res = Activator.CreateInstance(type);
            }

            var argTypes = new[] {StringType, type.MakeByRefType()};
            object[] args = {s, res};
            var tryParse = type.GetMethod("TryParse", argTypes);

            if (!(bool) (tryParse?.Invoke(null, args) ?? false))
                throw new InvalidCastException($"Cannot convert value to type {type.Name}.");

            return args[1];
        }

        #endregion


        #region OrderBy

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, SortableParams parameters)
        {
            var type = typeof(T);
            var param = Expression.Parameter(type, "x");

            var pi = GetProperty(type, parameters.OrderByField);
            var expr = Expression.Property(param, pi);
            var propertyType = pi.PropertyType;

            var delegateType = typeof(Func<,>).MakeGenericType(type, propertyType);
            var lambda = Expression.Lambda(delegateType, expr, param);

            var method = GetOrderByMethod(parameters.OrderByAscending);

            return ((IOrderedQueryable<T>) method.MakeGenericMethod(type, propertyType)
                .Invoke(null, new object[] {query, lambda}));
        }

        private static MethodInfo GetOrderByMethod(bool isAscending)
        {
            var sortDirection = isAscending ? "OrderBy" : "OrderByDescending";
            return QueryableType.GetMethods().Single(method => method.Name == sortDirection
                                                               && method.IsGenericMethodDefinition
                                                               && method.GetGenericArguments().Length == 2
                                                               && method.GetParameters().Length == 2);
        }

        #endregion OrderBy

        private static Expression GetConstant(object obj, Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Expression.Convert(Expression.Constant(obj), type);

            return Expression.Constant(obj);
        }

        private static PropertyInfo GetProperty(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new ArgumentException("{0} is not a valid property", propertyName);
            }

            return property;
        }
    }
}