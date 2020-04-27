using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using AutoMapper;
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

        public async Task<PaginationDto<TDto>> GetPageAsync<TDto, TEntity>(IQueryable<TEntity> query, FullPaginationQueryParams parameters)
            where TDto : class where TEntity : class
        {
            if (parameters.Filters != null)
            {
                query = query.Where(parameters.Filters);
            }
            if (parameters.Sort != null)
            {
                query = query.OrderBy(parameters.Sort);
            }
            return await GetPageAsync<TDto, TEntity>(query, (PageableParams)parameters);
        }

        public async Task<PaginationDto<TDto>> GetPageAsync<TDto, TEntity>(IQueryable<TEntity> query, PageableParams parameters)
            where TDto : class where TEntity : class
        {
            var wrapper = new PaginationDto<TDto>();
            if (parameters.FirstRequest)
            {
                wrapper.TotalCount = await query.CountAsync();
            }
            var pageResult = await query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).AsNoTracking().ToListAsync();
            wrapper.Page = _mapper.Map<List<TDto>>(pageResult);
            return wrapper;
        }
    }

}
