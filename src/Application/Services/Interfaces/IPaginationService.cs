using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Domain.RDBMS.Entities;

namespace Application.Services.Interfaces
{
    public interface IPaginationService
    {
        /// <summary>
        /// Takes IQueryable and creates Pagination wrapper around TDto based on QueryParameters
        /// </summary>
        /// <typeparam name="TDto">Data type to be returned wrapped into Pagination</typeparam>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="query">IQueryable to be paginated</param>
        /// <param name="parameters">Pagination and filtering is performed based on these query parameters</param>
        /// <returns>Returns data encapsulated in PaginationDto based on QueryParameters</returns>
        Task<PaginationDto<TDto>> GetPageAsync<TDto, TEntity>(IQueryable<TEntity> query, QueryParameters parameters)
            where TDto : class where TEntity : class;
    }
}