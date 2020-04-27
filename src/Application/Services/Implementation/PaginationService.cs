using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class PaginationService : IPaginationService
    {
        private readonly IMapper _mapper;
        private static readonly Type StringType = typeof(string);
        private static readonly Type QueryableType = typeof(Queryable);
        public PaginationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<PaginationDto<TDto>> GetPageAsync<TDto, TEntity>(IQueryable<TEntity> query, QueryParameters parameters)
            where TDto : class where TEntity : class
        {
            if (parameters.SearchQuery != null && parameters.SearchField != null)
            {
                query = GetFilteredQueryable(query, parameters.SearchField, parameters.SearchQuery);
            }
            if (parameters.OrderByField != null)
            {
                query = GetOrderedQueryable(query, parameters.OrderByField, parameters.OrderByAscending);
            }
            return await GetPageAsync<TDto, TEntity>(query, parameters.Page, parameters.PageSize, parameters.FirstRequest);
        }

        private  async Task<PaginationDto<TDto>> GetPageAsync<TDto, TEntity>(IQueryable<TEntity> query, int pageIndex, int pageSize, bool isFirstRequest)
            where TDto : class where TEntity : class
        {
            var wrapper = new PaginationDto<TDto>();
            if (isFirstRequest)
            {
                wrapper.TotalCount = await query.CountAsync();
            }
            var pageResult = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            wrapper.Page = _mapper.Map<List<TDto>>(pageResult);
            return wrapper;
        }
        private IQueryable<T> GetFilteredQueryable<T>(IQueryable<T> query, string property, string value)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var propertyExp = Expression.Property(param, property);
            var comparisonExp = Expression.Constant(value, StringType);
            var methodExp = GetStringMethod("Contains");
            var method = propertyExp.Type != StringType
                ? Expression.Call(Expression.Call(propertyExp, GetStringMethod("ToString")), methodExp,
                    comparisonExp)
                : Expression.Call(propertyExp, methodExp, comparisonExp);
            var lambda = Expression.Lambda<Func<T, bool>>(method, param);
            return query.Where(lambda);
        }
        private IOrderedQueryable<T> GetOrderedQueryable<T>(IQueryable<T> query, string property, bool sortAscending)
        {
            var type = typeof(T);
            var param = Expression.Parameter(type, "x");

            var pi = GetProperty<T>(property);
            var expr = Expression.Property(param, pi);
            var propertyType = pi.PropertyType;

            var delegateType = typeof(Func<,>).MakeGenericType(type, propertyType);
            var lambda = Expression.Lambda(delegateType, expr, param);

            var method = GetOrderByMethod(sortAscending);

            return ((IOrderedQueryable<T>)method.MakeGenericMethod(type, propertyType)
                .Invoke(null, new object[] { query, lambda }));
        }

        //Make Enum for GetStringMethod parameter?
        private static MethodInfo GetStringMethod(string methodName)
        {
            return StringType.GetMethod(methodName, new[] { StringType });
        }
        private static MethodInfo GetOrderByMethod(bool isAscending)
        {
            var sortDirection = isAscending ? "OrderBy" : "OrderByDescending";
            return QueryableType.GetMethods().Single(method => method.Name == sortDirection
                                                               && method.IsGenericMethodDefinition
                                                               && method.GetGenericArguments().Length == 2
                                                               && method.GetParameters().Length == 2);
        }
        private static PropertyInfo GetProperty<T>(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new ArgumentException("{0} is not a valid property", propertyName);
            }
            return property;
        }
    }

}
