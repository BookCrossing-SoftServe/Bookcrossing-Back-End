using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.QueryableExtension;

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
       /// <param name="fullPaginationQuery">QueryParameters containing page index, pageSize, searchQuery and if it's a first Request</param>
       /// <returns>Returns Pagination with Page result and Total amount of items</returns>
        Task<PaginationDto<AuthorDto>> GetAll(FullPaginationQueryParams fullPaginationQuery);
       
       /// <summary>
       /// Retrieve Authors by Ids
       /// </summary>
       /// <param name="ids">Author's IDs</param>
       /// <returns>list of authors</returns>
       Task<List<AuthorDto>> GetAll(int[] ids);

        /// <summary>
        /// Update specified Author
        /// </summary>
        /// <param name="author">Author's DTO instance</param>
        /// <returns></returns>
        Task<bool> Update(AuthorDto author);

        /// <summary>
        /// Remove author from database
        /// </summary>
        /// <param name="authorId">Author's ID</param>
        /// <returns></returns>
        Task<bool> Remove(int authorId);

        /// <summary>
        /// Create new author and add it into Database
        /// </summary>
        /// <param name="author">Author DTO instance</param>
        /// <returns>Returns created Author's DTO </returns>
        Task<AuthorDto> Add(AuthorDto author);
       
        /// <summary>
        /// Merges several authors into one
        /// </summary>
        /// <param name="mergeDto">Author and Authors to be merged</param>
        /// <returns>Author that replaced all merged authors</returns>
        Task<AuthorDto> Merge(AuthorMergeDto mergeDto);
        /// <summary>
        /// Get authors filtered by filter
        /// </summary>
        /// <param name="filter">string filter</param>
        /// <returns>Returns filtered authors </returns>
        Task<List<AuthorDto>> FilterAuthors(string filter);

    }
}
