using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Domain.RDBMS.Entities;

namespace Application.Services.Interfaces
{
    public interface IRequestService
    {
        /// <summary>
        /// Ability to request any book as user
        /// </summary>
        /// <param name="userId">User who wanna get a book</param>
        /// <param name="bookId">Certain book</param>
        /// <returns>Created request</returns>
        Task<RequestDto> Make(int userId, int bookId);

        /// <summary>
        /// Ability to get all requests for book
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="parameters">QueryParameters containing page index, pageSize, searchQuery and if it's a first Request</param>
        /// <returns>List of all requests DTO by book id in certain page</returns>
        Task<PaginationDto<RequestDto>> Get(Expression<Func<Request, bool>> predicate, BookQueryParams parameters);

        /// <summary>
        /// Ability to get first or last request for a certain book
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="query">Query params</param>
        /// <returns>RequestDto</returns>
        Task<RequestDto> GetByBook(Expression<Func<Request, bool>> predicate, RequestsQueryParams query);

        /// <summary>
        /// Ability to get all requests for a certain book
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>RequestDto</returns>
        Task<IEnumerable<RequestDto>> GetAllByBook(Expression<Func<Request, bool>> predicate);

        /// <summary>
        /// Ability to approve that user receiver his requested book
        /// </summary>
        /// <param name="id">Request Id</param>
        /// <returns>boolean</returns>
        Task<bool> ApproveReceive(int id);
       
        /// <summary>
        /// Remove request from database
        /// </summary>
        /// <param name="requestId">Request's ID</param>
        /// <returns>boolean</returns>
        Task<bool> Remove(int requestId);
    }
}
