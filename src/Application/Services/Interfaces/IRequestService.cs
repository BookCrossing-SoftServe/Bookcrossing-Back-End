using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.QueryableExtension;

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
        /// Ability to get all requests for your book in certain book
        /// </summary>
        /// <param name="bookId">Book`s id</param>
        /// <param name="fullPaginationQuery">QueryParameters containing page index, pageSize, searchQuery and if it's a first Request</param>
        /// <returns>List of all requests DTO by book id in certain page</returns>
        Task<PaginationDto<RequestDto>> Get(int bookId, FullPaginationQueryParams fullPaginationQuery);
        /// <summary>
        /// Ability to approve book request as book owner
        /// </summary>
        /// <param name="id">Request Id</param>
        /// <returns>boolean</returns>
        Task<bool> Approve(int id);
       
        /// <summary>
        /// Remove request from database
        /// </summary>
        /// <param name="requestId">Request's ID</param>
        /// <returns>boolean</returns>
        Task<bool> Remove(int requestId);
    }
}
