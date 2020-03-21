using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Entities = Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IAuthor
    {
        /// <summary>
        /// Retrieve Author by ID
        /// </summary>
        /// <param name="authorId">Author's ID</param>
        /// <returns>returns Author DTO</returns>
        Task<AuthorDto> GetById(int authorId);

        /// <summary>
        /// Retrieve all Authors
        /// </summary>
        /// <returns>returns list of Author DTOs</returns>
        Task<List<AuthorDto>> GetAll();

        /// <summary>
        /// Update specified Author
        /// </summary>
        /// <param name="author">Author's DTO instance</param>
        /// <returns></returns>
        Task Update(AuthorDto author);

        /// <summary>
        /// Remove specified Author
        /// </summary>
        /// <param name="author">Author's DTO instance</param>
        /// <returns></returns>
        Task<AuthorDto> Remove(int authorId);

        /// <summary>
        /// Create new author and add it into DbContext
        /// </summary>
        /// <param name="author">Author's DTO instance</param>
        /// <returns></returns>
        Task<int> Add(AuthorDto author);
    }
}
