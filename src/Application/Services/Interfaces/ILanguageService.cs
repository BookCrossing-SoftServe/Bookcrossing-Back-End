using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;

namespace Application.Services.Interfaces
{
    public interface ILanguageService
    {
        /// <summary>
        /// Retrieve language by ID
        /// </summary>
        /// <param name="languageId">Languages ID</param>
        /// <returns>returns Language DTO</returns>
        Task<LanguageDto> GetById(int languageId);

        /// <summary>
        /// Retrieve all languages
        /// </summary>
        /// <returns>returns list of Genres DTOs</returns>
        Task<List<LanguageDto>> GetAll();

        /// <summary>
        /// Retrieve Pagination for Genre
        /// </summary>
        /// <param name="fullPaginationQuery">QueryParameters containing page index, pageSize, searchQuery and if it's a first Request</param>
        /// <returns>Returns Pagination with Page result and Total amount of items</returns>
        Task<PaginationDto<LanguageDto>> GetAll(FullPaginationQueryParams fullPaginationQuery);

        /// <summary>
        /// Update specified language
        /// </summary>
        /// <param name="language">Languages DTO instance</param>
        /// <returns></returns>
        Task<bool> Update(LanguageDto language);

        /// <summary>
        /// Remove language from database
        /// </summary>
        /// <param name="languageId">Language ID</param>
        /// <returns>Returns removed languages DTO</returns>
        Task<bool> Remove(int languageId);

        /// <summary>
        /// Create new language and add it into Database
        /// </summary>
        /// <param name="language">NewGenre DTO instance</param>
        /// <returns>Returns created Languages DTO </returns>
        Task<LanguageDto> Add(LanguageDto language);
    }
}
