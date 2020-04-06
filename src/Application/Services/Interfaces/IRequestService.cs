using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;

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
        /// Ability to loook at all requests for your book
        /// </summary>
        /// <param name="bookId">Book`s id</param>
        /// <returns>List of all requests DTO by book id</returns>
        IEnumerable<RequestDto> Get(int bookId);
        /// <summary>
        /// Ability to approve book request as book owner
        /// </summary>
        /// <param name="id">Request Id</param>
        /// <returns>Approved request</returns>
        Task<RequestDto> Approve(int id);
       
        /// <summary>
        /// Remove request from database
        /// </summary>
        /// <param name="requestId">Request's ID</param>
        /// <returns>Returns removed request's DTO</returns>
        Task<RequestDto> Remove(int requestId);
    }
}
