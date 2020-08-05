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
        Task<BookGetDto> GetByIdAsync(int bookId);

        /// <summary>
        /// Retrieve all books
        /// </summary>
        /// <returns>returns list of Book DTOs</returns>
        Task<PaginationDto<BookGetDto>> GetAllAsync(BookQueryParams parameters);

        /// <summary>
        /// Update specified book
        /// </summary>
        /// <param name="book">Book DTO instance</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(BookPutDto book);

        /// <summary>
        /// Remove book from database
        /// </summary>
        /// <param name="bookId">Book's ID</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(int bookId);

        /// <summary>
        /// Create new book and add it into Database
        /// </summary>
        /// <param name="book">Book DTO instance</param>
        /// <returns>Returns inserted Book's ID</returns>
        Task<BookGetDto> AddAsync(BookPostDto book);

        /// <summary>
        /// Retrieve books registered by user
        /// </summary>
        ///  <param name="parameters">filter parametrs</param>
        /// <returns></returns>
        Task<PaginationDto<BookGetDto>> GetRegistered(BookQueryParams parameters);

        /// <summary>
        /// Retrieve books current owned by user
        /// </summary>
        ///  <param name="parameters">filter parametrs</param>
        /// <returns></returns>
        Task<PaginationDto<BookGetDto>> GetCurrentOwned(BookQueryParams parameters);

        /// <summary>
        /// Retrieve books read by current user
        /// </summary>
        ///  <param name="parameters">filter parametrs</param>
        /// <returns></returns>
        /// 

        Task<List<BookGetDto>> GetCurrentOwnedById(int id);


        Task<int> GetCurrentOwnedByIdCount(int userId);

        Task<PaginationDto<BookGetDto>> GetReadBooksAsync(BookQueryParams parameters);

        /// <summary>
        /// Change book`s status to available
        /// </summary>
        ///  <param name="bookId">Book Id</param>
        /// <returns></returns>
        Task<bool> ActivateAsync(int bookId);

        /// <summary>
        /// Change book`s status to InActive
        /// </summary>
        ///  <param name="bookId">Book Id</param>
        /// <returns></returns>
        Task<bool> DeactivateAsync(int bookId);
    }
}
