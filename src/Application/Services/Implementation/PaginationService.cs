using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using AutoMapper;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class PaginationService : IPaginationService
    {
        private readonly IMapper _mapper;
        public PaginationService(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public async Task<PaginationDto<TDto>> GetPage<TDto, TEntity>(IQueryable<TEntity> query, QueryParameters parameters) 
            where TDto : class where TEntity : class
        {
            if (parameters.SearchQuery != null && parameters.SearchField != null)
            {
                ValidateProperty<TDto>(parameters.SearchField);
                var condition = GetExpressionContains<TEntity>(parameters.SearchField, parameters.SearchQuery);
                query = query.Where(condition);
            }
            if (parameters.OrderByField != null)
            {
                ValidateProperty<TDto>(parameters.OrderByField);
                query = Filter(query, parameters.OrderByField, parameters.OrderByAscending);
            }
            var wrapper = new PaginationDto<TDto>();
            if (parameters.FirstRequest)
            {
                wrapper.TotalCount = await query.CountAsync();
            }
            var pageResult = await query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();
            wrapper.Page = _mapper.Map<List<TDto>>(pageResult);
            return wrapper;
        }

        private static void ValidateProperty<TDto>(string property)
        {
            var dtoProperty = typeof(TDto).GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (dtoProperty == null)
            {
                throw new ArgumentException("{0} is not a valid property", property);
            }
        }
        private static Expression<Func<TEntity, bool>> GetExpressionContains<TEntity>(string property, string value)
        {
            var stringType = typeof(string);
            var param = Expression.Parameter(typeof(TEntity), "e");
            var propertyExp = Expression.Property(param, property);
            var comparisonExp = Expression.Constant(value, stringType);
            var methodExp = stringType.GetMethod("Contains", new[] { stringType });

            var method = propertyExp.Type != typeof(string)
                ? Expression.Call(Expression.Call(propertyExp, typeof(object).GetMethod("ToString")), methodExp,
                    comparisonExp)
                : Expression.Call(propertyExp, methodExp, comparisonExp);

            return Expression.Lambda<Func<TEntity, bool>>(method, param);
        }
        private static IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query, string property, bool sortAscending)
        {
            var ordering = sortAscending ? property + " ascending" : property + " descending";
            return query.OrderBy(ordering);
        }
    }
}
