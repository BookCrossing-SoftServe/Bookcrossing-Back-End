using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IBook
    {
        /// <summary>
        /// Retrieve book by ID
        /// </summary>
        /// <param name="bookId">Book's ID</param>
        /// <returns>returns Book DTO</returns>
        Task<BookDto> GetById(int bookId);

        /// <summary>
        /// Retrieve all books
        /// </summary>
        /// <returns>returns list of Book DTOs</returns>
        Task<List<BookDto>> GetAll();

        /// <summary>
        /// Update specified book
        /// </summary>
        /// <param name="book">Book DTO instance</param>
        /// <returns></returns>
        Task Update(BookDto book);

        /// <summary>
        /// Remove book from database
        /// </summary>
        /// <param name="bookId">Book's ID</param>
        /// <returns>Returns removed Book DTO</returns>
        Task<BookDto> Remove(int bookId);

        /// <summary>
        /// Create new book and add it into Database
        /// </summary>
        /// <param name="book">Book DTO instance</param>
        /// <returns>Returns inserted Book's ID</returns>
        Task<int> Add(BookDto book);
    }
}
