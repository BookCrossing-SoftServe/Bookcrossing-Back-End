using Application.Dto;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.RDBMS.Entities;
using Domain.RDBMS;
using System.Linq;
using LinqKit;
using System.Reflection.Metadata.Ecma335;
using Application.Dto.QueryParams;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using Infrastructure.RDBMS;
using System;

namespace Application.Services.Implementation
{
    public class BookService : Interfaces.IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        private readonly IRepository<UserLocation> _userLocationRepository;
        private readonly IRepository<Request> _requestRepository;
        private readonly IUserResolverService _userResolverService;
        private readonly IPaginationService _paginationService;
        private readonly IImageService _imageService;
        private readonly BookCrossingContext _context;
        private readonly IMapper _mapper;

        public BookService(IRepository<Book> bookRepository, IMapper mapper, IRepository<BookAuthor> bookAuthorRepository, IRepository<BookGenre> bookGenreRepository,
            IRepository<UserLocation> userLocationRepository, IPaginationService paginationService, IRepository<Request> requestRepository, BookCrossingContext context, 
            IUserResolverService userResolverService, IImageService imageService)
        {
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookGenreRepository = bookGenreRepository;
            _userLocationRepository = userLocationRepository;
            _requestRepository = requestRepository;
            _paginationService = paginationService;
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _userResolverService = userResolverService;
        }

        public async Task<BookGetDto> GetById(int bookId)
        {
            return _mapper.Map<BookGetDto>(await _bookRepository.GetAll()
                                                               .Include(p => p.BookAuthor)
                                                               .ThenInclude(x => x.Author)
                                                               .Include(p => p.BookGenre)
                                                               .ThenInclude(x => x.Genre)
                                                               .Include(p => p.User)
                                                               .ThenInclude(x => x.UserLocation)
                                                               .ThenInclude(x => x.Location)
                                                               .FirstOrDefaultAsync(p => p.Id == bookId));
        }
        public async Task<PaginationDto<BookGetDto>> GetAll(BookQueryParams parameters)
        {

            var books = _bookRepository.GetAll();
            var author = _bookAuthorRepository.GetAll();
            if (parameters.SearchTerm != null)
            {
                var term = parameters.SearchTerm.Split(" ");
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
            if (parameters.Genres != null)
            {
                var predicate = PredicateBuilder.New<BookGenre>();
                foreach (var id in parameters.Genres)
                {
                    var tempId = id;
                    predicate = predicate.Or(g => g.Genre.Id == tempId);
                }
                genre = genre.Where(predicate);
            }

            if (parameters.ShowAvailable == true)
            {
                books = books.Where(b => b.Available);
            }

            var location = _userLocationRepository.GetAll();
            if (parameters.location != null)
            {
                location = location.Where(l => l.Location.Id == parameters.location);
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

            return await _paginationService.GetPageAsync<BookGetDto, Book>(query, parameters);
        }

        public async Task<BookGetDto> Add(BookPostDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            if (bookDto.Image != null)
            {
                book.ImagePath = await _imageService.UploadImage(bookDto.Image);
            }
            _bookRepository.Add(book);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookGetDto>(book);
        }

        public async Task<bool> Remove(int bookId)
        {
            var book = await _bookRepository.GetAll()
                            .FirstOrDefaultAsync(p => p.Id == bookId);
            if (book == null)
                return false;
            _imageService.DeleteImage(book.ImagePath);
            _bookRepository.Remove(book);
            var affectedRows = await _bookRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> Update(BookPutDto bookDto)
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

        public async Task<PaginationDto<BookGetDto>> GetRegistered(BookQueryParams parameters)
        {
            //filtration books
            var userId = _userResolverService.GetUserId();
            var books = _bookRepository.GetAll();
            var author = _bookAuthorRepository.GetAll();
            if (parameters.SearchTerm != null)
            {
                var term = parameters.SearchTerm.Split(" ");
                if (term.Length <= 1)
                {
                    author = author.Where(a =>
                        a.Author.FirstName.Contains(term[0]) || a.Author.LastName.Contains(term[0]) || a.Book.Name.Contains(parameters.SearchTerm));
                }
                else
                {
                    author = author.Where(a =>
                        a.Author.FirstName.Contains(term[0]) && a.Author.LastName.Contains(term[term.Length - 1]) || a.Book.Name.Contains(parameters.SearchTerm));
                }
            }

            var genre = _bookGenreRepository.GetAll();
            if (parameters.Genres != null)
            {
                var predicate = PredicateBuilder.New<BookGenre>();
                foreach (var id in parameters.Genres)
                {
                    var tempId = id;
                    predicate = predicate.Or(g => g.Genre.Id == tempId);
                }
                genre = genre.Where(predicate);
            }

            if (parameters.ShowAvailable == true)
            {
                books = books.Where(b => b.Available);
            }

            var location = _userLocationRepository.GetAll();
            if (parameters.location != null)
            {
                location = location.Where(l => l.Location.Id == parameters.location);
            }
            var bookIds =
                from b in books
                join g in genre on b.Id equals g.BookId
                join a in author on b.Id equals a.BookId
                join l in location on b.UserId equals l.UserId
                select b.Id;

            //registered books            
            var allRequests = await _requestRepository.GetAll()
                                              .Select(x => new { Owner = x.Owner.Id, Time = x.RequestDate, Book = x.Book })
                                              .ToListAsync();

            var firstRequests = allRequests.GroupBy(a => new { a.Book })
                                           .Select(g => new
                                           {
                                               g.Key.Book,
                                               MinTime = g.Min(book => book.Time)
                                           }).ToList();

            var firstRequestsBookId = firstRequests.Select(g => g.Book.Id);

            var userBooks = await _bookRepository.GetAll()
                                       .Where(x => x.UserId == userId)
                                       .Select(x => x.Id)
                                       .ToListAsync();

            var userCurrentBooks = userBooks.Except(firstRequestsBookId);

            var userFirstBooks = allRequests.Where(a => a.Owner == userId && a.Time == firstRequests.Single(b => b.Book == a.Book).MinTime).Select(a => a.Book.Id);
            //all user books
            var allBooks = userCurrentBooks.Union(userFirstBooks);

            var query = _bookRepository.GetAll().Where(x => bookIds.Contains(x.Id))
                .Where(x => allBooks.Contains(x.Id))
                .Include(p => p.BookAuthor)
                .ThenInclude(x => x.Author)
                .Include(p => p.BookGenre)
                .ThenInclude(x => x.Genre)
                .Include(p => p.User)
                .ThenInclude(x => x.UserLocation)
                .ThenInclude(x => x.Location);

            return await _paginationService.GetPageAsync<BookGetDto, Book>(query, parameters);
        }

        public async Task<PaginationDto<BookGetDto>> GetCurrentOwned(BookQueryParams parameters)
        {
            var userId = _userResolverService.GetUserId();
            var books = _bookRepository.GetAll();
            var author = _bookAuthorRepository.GetAll();
            if (parameters.SearchTerm != null)
            {
                var term = parameters.SearchTerm.Split(" ");
                if (term.Length <= 1)
                {
                    author = author.Where(a =>
                        a.Author.FirstName.Contains(term[0]) || a.Author.LastName.Contains(term[0]) || a.Book.Name.Contains(parameters.SearchTerm));
                }
                else
                {
                    author = author.Where(a =>
                        a.Author.FirstName.Contains(term[0]) && a.Author.LastName.Contains(term[term.Length - 1]) || a.Book.Name.Contains(parameters.SearchTerm));
                }
            }

            var genre = _bookGenreRepository.GetAll();
            if (parameters.Genres != null)
            {
                var predicate = PredicateBuilder.New<BookGenre>();
                foreach (var id in parameters.Genres)
                {
                    var tempId = id;
                    predicate = predicate.Or(g => g.Genre.Id == tempId);
                }
                genre = genre.Where(predicate);
            }

            if (parameters.ShowAvailable == true)
            {
                books = books.Where(b => b.Available);
            }

            var location = _userLocationRepository.GetAll();
            if (parameters.location != null)
            {
                location = location.Where(l => l.Location.Id == parameters.location);
            }
            var bookIds =
                from b in books
                join g in genre on b.Id equals g.BookId
                join a in author on b.Id equals a.BookId
                join l in location on b.UserId equals l.UserId
                select b.Id;

            var query = _bookRepository.GetAll().Where(x => bookIds.Contains(x.Id))
                .Where(p=>p.UserId==userId)
                .Include(p => p.BookAuthor)
                .ThenInclude(x => x.Author)
                .Include(p => p.BookGenre)
                .ThenInclude(x => x.Genre)
                .Include(p => p.User)
                .ThenInclude(x => x.UserLocation)
                .ThenInclude(x => x.Location);

            return await _paginationService.GetPageAsync<BookGetDto, Book>(query, parameters);
        }
    }
}
