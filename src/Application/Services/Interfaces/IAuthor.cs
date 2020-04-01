using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;

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
        /// Remove author from database
        /// </summary>
        /// <param name="authorId">Author's ID</param>
        /// <returns>Returns removed author's DTO</returns>
        Task<bool> Remove(int authorId);

        /// <summary>
        /// Create new author and add it into Database
        /// </summary>
        /// <param name="author">NewAuthor DTO instance</param>
        /// <returns>Returns created Author's DTO </returns>
        Task<AuthorDto> Add(NewAuthorDto author);

    }
}
