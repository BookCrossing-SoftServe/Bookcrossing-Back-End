using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface IAuthorService
    {
        /// <summary>
        /// Retrieve Author by ID
        /// </summary>
        /// <param name="authorId">Author's ID</param>
        /// <returns>returns Author DTO</returns>
        Task<AuthorDto> GetById(int authorId);

       /// <summary>
       /// Retrieve Pagination for Author
       /// </summary>
       /// <param name="query">QueryParameters containing page index, pageSize, searchQuery and if it's a first Request</param>
       /// <returns>Returns Pagination with Page result and Total amount of items</returns>
        Task<PaginationDto<AuthorDto>> GetAuthors(QueryParameters query);

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
