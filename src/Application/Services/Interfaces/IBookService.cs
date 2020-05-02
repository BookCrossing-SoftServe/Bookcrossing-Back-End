using Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto.QueryParams;

namespace Application.Services.Interfaces
{
    public interface IBookService
    {
        /// <summary>
        /// Retrieve book by ID
        /// </summary>
        /// <param name="bookId">Book's ID</param>
        /// <returns>returns Book DTO</returns>
        Task<BookDetailsDto> GetById(int bookId);

        /// <summary>
        /// Retrieve all books
        /// </summary>
        /// <returns>returns list of Book DTOs</returns>
        Task<PaginationDto<BookDetailsDto>> GetAll(BookQueryParams parameters);

        /// <summary>
        /// Update specified book
        /// </summary>
        /// <param name="book">Book DTO instance</param>
        /// <returns></returns>
        Task<bool> Update(BookDto book);

        /// <summary>
        /// Remove book from database
        /// </summary>
        /// <param name="bookId">Book's ID</param>
        /// <returns></returns>
        Task<bool> Remove(int bookId);

        /// <summary>
        /// Create new book and add it into Database
        /// </summary>
        /// <param name="book">Book DTO instance</param>
        /// <returns>Returns inserted Book's ID</returns>
        Task<BookDto> Add(AddBookDto book);
    }
}
