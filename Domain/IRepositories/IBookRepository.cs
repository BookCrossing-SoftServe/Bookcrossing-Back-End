using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IBookRepository
    {
        /// <summary>
        /// Returns all books list
        /// </summary>
        /// <returns></returns>
        IEnumerable<Book> GetAllBooks();
        /// <summary>
        /// Get Book by Id
        /// </summary>
        /// <param name="bookId">Book Id</param>
        /// <returns></returns>
        Book GetBookById(int bookId);
        /// <summary>
        /// Get all User`s books
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        List<Book> GetBookByUser(User user);
        /// <summary>
        /// Add new Book
        /// </summary>
        /// <param name="user">Book</param>
        void AddNewBook(Book book);
    }
}
