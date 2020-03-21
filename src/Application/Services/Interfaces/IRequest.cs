using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IRequest
    {
        /// <summary>
        /// Ability to request any book as user
        /// </summary>
        /// <param name="userId">User who wanna get a book</param>
        /// <param name="bookId">Certain book</param>
        /// <returns></returns>
        Task Make(int userId, int bookId);
        /// <summary>
        /// Ability to loook at all requests for your book
        /// </summary>
        /// <param name="bookId">Book id</param>
        /// <returns></returns>
        Task<IEnumerable<Request>> Get(int bookId);
        /// <summary>
        /// Ability to approve book request as book owner
        /// </summary>
        /// <param name="id">Request Id</param>
        /// <returns></returns>
        Task Approve(int id);
    }
}
