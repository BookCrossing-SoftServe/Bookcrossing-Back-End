using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS.Entities;
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
            var wrapper = new PaginationDto<TDto>();
            if (parameters.FirstRequest)
            {
                wrapper.TotalCount = await query.CountAsync();
            }
            var pageResult = await query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();
            wrapper.Page = _mapper.Map<List<TDto>>(pageResult);
            return wrapper;
        }
    }
}
