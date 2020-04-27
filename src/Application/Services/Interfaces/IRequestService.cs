using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Dto;
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
        /// <param name="query">QueryParameters containing page index, pageSize, searchQuery and if it's a first Request</param>
        /// <returns>List of all requests DTO by book id in certain page</returns>
        Task<PaginationDto<RequestDto>> Get(Expression<Func<Request, bool>> predicate, QueryParameters query);

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
