using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;

namespace Infastructure.Reposetories
{
    public class BookRepository : IBookRepository
    {
        public readonly BookCrossingContext _context;
        public BookRepository(BookCrossingContext context)
        {
            _context = context;
        }

        public void AddNewBook(Book book)
        {
            _context.Book.Add(book);
            _context.SaveChanges();
        }

        public IEnumerable<Book> GetAllBooks() => _context.Book.ToList();

        public Book GetBookById(int bookId) =>
            _context.Book.FirstOrDefault(p => p.Id == bookId);

        public List<Book> GetBookByUser(User user) => _context.Book.Where(i => i.UserId == user.Id).ToList();
    }
}
