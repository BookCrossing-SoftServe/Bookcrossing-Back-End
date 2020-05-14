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
        private readonly IMapper _mapper;

        public BookService(IRepository<Book> bookRepository, IMapper mapper, IRepository<BookAuthor> bookAuthorRepository, IRepository<BookGenre> bookGenreRepository,
            IRepository<UserLocation> userLocationRepository, IPaginationService paginationService, IRepository<Request> requestRepository,
            IUserResolverService userResolverService, IImageService imageService)
        {
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookGenreRepository = bookGenreRepository;
            _userLocationRepository = userLocationRepository;
            _requestRepository = requestRepository;
            _paginationService = paginationService;
            _mapper = mapper;
            _imageService = imageService;
            _userResolverService = userResolverService;
        }

        public async Task<BookGetDto> GetByIdAsync(int bookId)
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

        public async Task<BookGetDto> AddAsync(BookPostDto bookDto)
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

        public async Task<bool> RemoveAsync(int bookId)
        {
            var book = await _bookRepository.FindByIdAsync(bookId);
            if (book == null)
            {
                return false;
            }
            if (book.ImagePath != null)
            {
                _imageService.DeleteImage(book.ImagePath);
            }
            _bookRepository.Remove(book);
            var affectedRows = await _bookRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> UpdateAsync(BookPutDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            var oldBook = await _bookRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(a => a.Id == book.Id);
            if (oldBook == null)
            {
                return false;
            }
            if (bookDto.FieldMasks.Contains("Image"))
            {
                string imagePath;
                bookDto.FieldMasks.Remove("Image");
                bookDto.FieldMasks.Add("ImagePath");
                if (oldBook.ImagePath != null)
                {
                    _imageService.DeleteImage(oldBook.ImagePath);
                }
                if (bookDto.Image != null)
                {
                    imagePath = await _imageService.UploadImage(bookDto.Image);
                }
                else
                {
                    imagePath = null;
                }
                book.ImagePath = imagePath;
            }
            await _bookRepository.Update(book, bookDto.FieldMasks);
            var affectedRows = await _bookRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<PaginationDto<BookGetDto>> GetAllAsync(BookQueryParams parameters)
        {
            var query = GetFilteredQuery(_bookRepository.GetAll(), parameters);
            return await _paginationService.GetPageAsync<BookGetDto, Book>(query, parameters);
        }
        public async Task<PaginationDto<BookGetDto>> GetRegistered(BookQueryParams parameters)
        {
            
            var userId = _userResolverService.GetUserId();
                       
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

            var query = _bookRepository.GetAll().Where(x => allBooks.Contains(x.Id));
            query = GetFilteredQuery(query, parameters);

            return await _paginationService.GetPageAsync<BookGetDto, Book>(query, parameters);
        }

        public async Task<PaginationDto<BookGetDto>> GetCurrentOwned(BookQueryParams parameters)
        {
            var userId = _userResolverService.GetUserId();
            var query = _bookRepository.GetAll().Where(p => p.UserId == userId);
            query = GetFilteredQuery(query, parameters);

            return await _paginationService.GetPageAsync<BookGetDto, Book>(query, parameters);
        }

        public async Task<PaginationDto<BookGetDto>> GetReadBooksAsync(BookQueryParams parameters)
        {
            var userId = _userResolverService.GetUserId();
            var ownedBooks = _requestRepository.GetAll().Where(a => a.OwnerId == userId).Select(a => a.Book);
            var currentlyOwnedBooks = _bookRepository.GetAll().Where(a => a.UserId == userId);
            var readBooks = ownedBooks.Union(currentlyOwnedBooks);
            var query = GetFilteredQuery(readBooks, parameters);
            return await _paginationService.GetPageAsync<BookGetDto, Book>(query, parameters);
        }

        private IQueryable<Book> GetFilteredQuery(IQueryable<Book> query, BookQueryParams parameters)
        {
            if (parameters.ShowAvailable == true)
            {
                query = query.Where(b => b.Available);
            }
            if (parameters.Location != null)
            {
                query = query.Where(x => x.User.UserLocation.Any(l => l.Location.Id == parameters.Location));
            }
            if (parameters.SearchTerm != null)
            {
                var term = parameters.SearchTerm.Split(" ");
                if (term.Length == 1)
                {
                    query = query.Where(x => x.Name.Contains(parameters.SearchTerm) || x.BookAuthor.Any(a => a.Author.LastName.Contains(term[term.Length - 1]) || a.Author.FirstName.Contains(term[0])));
                }
                else
                {
                    query = query.Where(x => x.Name.Contains(parameters.SearchTerm) || x.BookAuthor.Any(a => a.Author.LastName.Contains(term[term.Length - 1]) && a.Author.FirstName.Contains(term[0])));
                }
            }
            if (parameters.Genres != null)
            {
                var predicate = PredicateBuilder.New<Book>();
                foreach (var id in parameters.Genres)
                {
                    var tempId = id;
                    predicate = predicate.Or(g => g.BookGenre.Any(g => g.Genre.Id == tempId));
                }
                query = query.Where(predicate);
            }


            var location = _userLocationRepository.GetAll();
            var author = _bookAuthorRepository.GetAll();
            var genre = _bookGenreRepository.GetAll();
            var bookIds =
                from b in query
                join g in genre on b.Id equals g.BookId
                join a in author on b.Id equals a.BookId
                join l in location on b.UserId equals l.UserId
                select b.Id;

            return query.Where(x => bookIds.Contains(x.Id))
                .Include(p => p.BookAuthor)
                .ThenInclude(x => x.Author)
                .Include(p => p.BookGenre)
                .ThenInclude(x => x.Genre)
                .Include(p => p.User)
                .ThenInclude(x => x.UserLocation)
                .ThenInclude(x => x.Location);
        }

    }
}
