using Application.Dto;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.RDBMS.Entities;
using Domain.RDBMS;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Application.Dto.QueryParams;
using Application.Dto.QueryParams.Enums;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using Infrastructure.RDBMS;

namespace Application.Services.Implementation
{
    public class BookService : Interfaces.IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        private readonly IRepository<UserLocation> _userLocationRepository;
        private readonly IPaginationService _paginationService;
        private readonly BookCrossingContext _context;
        private readonly IMapper _mapper;
        public BookService(IRepository<Book> bookRepository, IMapper mapper, IRepository<BookAuthor> bookAuthorRepository, IRepository<BookGenre> bookGenreRepository, IRepository<UserLocation> userLocationRepository, IPaginationService paginationService, BookCrossingContext context)
        {
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookGenreRepository = bookGenreRepository;
            _userLocationRepository = userLocationRepository;
            _paginationService = paginationService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookDetailsDto> GetById(int bookId)
        {
            return _mapper.Map<BookDetailsDto>(await _bookRepository.GetAll()
                                                               .Include(p => p.BookAuthor)
                                                               .ThenInclude(x => x.Author)
                                                               .Include(p => p.BookGenre)
                                                               .ThenInclude(x => x.Genre)
                                                               .Include(p => p.User)
                                                               .ThenInclude(x => x.UserLocation)
                                                               .ThenInclude(x => x.Location)
                                                               .FirstOrDefaultAsync(p => p.Id == bookId));
        }
        public async Task<PaginationDto<BookDetailsDto>> GetAll(BookQueryParams parameters)
        {

            var books = _bookRepository.GetAll();
            var author = _bookAuthorRepository.GetAll();
            if (parameters.SearchTerm != null)
            {
                var term = parameters.SearchTerm.Split("");
                if (term.Length <= 1)
                {
                    author = author.Where(a =>
                        a.Author.FirstName.Contains(term[0]) || a.Author.LastName.Contains(term[0]) || a.Book.Name.Contains(parameters.SearchTerm));
                }
                else
                {
                    author = author.Where(a =>
                        a.Author.FirstName.Contains(term[0]) && a.Author.LastName.Contains(term[term.Length-1]) || a.Book.Name.Contains(parameters.SearchTerm));
                }
            }

            var genre = _bookGenreRepository.GetAll();
            if (parameters.GenreIds != null)
            {
                foreach (var id in parameters.GenreIds)
                {
                    genre = genre.Where(g => g.Genre.Id == id);
                }
            }

            if (parameters.ShowAvailable == true)
            {
                books = books.Where(b => b.Available);
            }

            var location = _userLocationRepository.GetAll();
            if (parameters.SelectedLocationId != null)
            {
                location = location.Where(l => l.Location.Id == parameters.SelectedLocationId);
            }
            var bookIds =
                from b in books
                join g in genre on b.Id equals g.BookId
                join a in author on b.Id equals a.BookId
                join l in location on b.UserId equals l.UserId
                select b.Id;

            var query = _bookRepository.GetAll().Where(x => bookIds.Contains(x.Id))
                .Include(p => p.BookAuthor)
                .ThenInclude(x => x.Author)
                .Include(p => p.BookGenre)
                .ThenInclude(x => x.Genre)
                .Include(p => p.User)
                .ThenInclude(x => x.UserLocation)
                .ThenInclude(x => x.Location);

            return await _paginationService.GetPageAsync<BookDetailsDto, Book>(query, parameters);


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
    }
}
