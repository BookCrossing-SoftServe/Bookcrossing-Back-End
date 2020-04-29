using Application.Dto;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.RDBMS.Entities;
using Domain.RDBMS;
using System.Linq;
using Infrastructure.RDBMS;

namespace Application.Services.Implementation
{
    public class BookService : Interfaces.IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        private readonly BookCrossingContext _context;
        private readonly IRepository<Request> _requestRepository;
        private readonly IMapper _mapper;
        public BookService(IRepository<Book> bookRepository, IMapper mapper, IRepository<BookAuthor> bookAuthorRepository,
            IRepository<BookGenre> bookGenreRepository, IRepository<Request> requestRepository, BookCrossingContext context)
        {
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookGenreRepository = bookGenreRepository;
            _requestRepository = requestRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookDto> GetById(int bookId)
        {
            return _mapper.Map<BookDto>(await _bookRepository.GetAll()
                                                               .Include(p => p.BookAuthor)
                                                               .ThenInclude(x => x.Author)
                                                               .Include(p => p.BookGenre)
                                                               .ThenInclude(x => x.Genre)
                                                               .FirstOrDefaultAsync(p => p.Id == bookId));
        }

        public async Task<List<BookDto>> GetAll()
        {
            return _mapper.Map<List<BookDto>>(await _bookRepository.GetAll()
                                                                    .Include(p => p.BookAuthor)
                                                                    .ThenInclude(x => x.Author)
                                                                    .Include(p => p.BookGenre)
                                                                    .ThenInclude(x => x.Genre)
                                                                    .ToListAsync());
        }

        public async Task<BookDto> Add(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _bookRepository.Add(book);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> Remove(int bookId)
        {
            var book = await _bookRepository.GetAll()
                            .Include(p => p.BookAuthor)
                            .ThenInclude(x => x.Author)
                            .Include(p => p.BookGenre)
                            .ThenInclude(x => x.Genre)
                            .FirstOrDefaultAsync(p => p.Id == bookId);
            if (book == null)
                return false;
            _bookRepository.Remove(book);
            var affectedRows = await _bookRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> Update(BookDto bookDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var book = _mapper.Map<Book>(bookDto);
                var doesBookExist = await _bookRepository.GetAll().AnyAsync(a => a.Id == book.Id);
                if (!doesBookExist)
                {
                    return false;
                }
                _bookAuthorRepository.RemoveRange(await _bookAuthorRepository.GetAll().Where(a => a.BookId == book.Id).ToListAsync());
                _bookGenreRepository.RemoveRange(await _bookGenreRepository.GetAll().Where(a => a.BookId == book.Id).ToListAsync());
                await _bookRepository.SaveChangesAsync();
                _bookAuthorRepository.AddRange(book.BookAuthor);
                _bookGenreRepository.AddRange(book.BookGenre);
                _bookRepository.Update(book);
                var affectedRows = await _bookRepository.SaveChangesAsync();
                transaction.Commit();
                return affectedRows > 0;
            }
        }

        public async Task<List<BookDto>> GetRegistered()
        {
            int userId = 1;

            var filteredRequests = await _requestRepository.GetAll()
                                           .Select(g => new { Book = g.Book, Time = g.RequestDate })
                                            .ToListAsync();

            var groupedRequests = filteredRequests.GroupBy(a => new { a.Book })
                                           .Select(g => new
                                           {
                                               g.Key.Book,
                                               MinTime = g.Min(book => book.Time)
                                           }).ToList();

            var bookId = groupedRequests.Select(g => g.Book.Id);

            var userBooks = await _bookRepository.GetAll()
                                       .Where(x => x.UserId == userId)
                                       .Select(x => x.Id)
                                       .ToListAsync();

            var currentBooks = userBooks.Except(bookId);

            var requests = await _requestRepository.GetAll()
                                              .Select(x => new { Owner = x.Owner.Id, Time = x.RequestDate, Book = x.Book })
                                              .ToListAsync();

            var userRequests = (from book in requests
                            join bt in groupedRequests
                              on book.Book equals bt.Book
                            where book.Time == bt.MinTime
                            && book.Owner == userId
                            select new { Book = book.Book.Id });

            //all user books
            var allBooks = currentBooks.Union(userRequests.Select(x => x.Book));

            return _mapper.Map<List<BookDto>>(await _bookRepository.GetAll().Where(x => allBooks.Contains(x.Id))
                                                                    .Include(p => p.BookAuthor)
                                                                    .ThenInclude(x => x.Author)
                                                                    .Include(p => p.BookGenre)
                                                                    .ThenInclude(x => x.Genre)
                                                                    .ToListAsync());
        }
    }
}
