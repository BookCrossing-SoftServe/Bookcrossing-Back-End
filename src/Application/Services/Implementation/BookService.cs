using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Email;
using Application.Dto.QueryParams;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace Application.Services.Implementation
{
    public class BookService : Interfaces.IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        private readonly IRepository<Language> _bookLanguageRepository;
        private readonly IRepository<User> _userLocationRepository;
        private readonly IRepository<Request> _requestRepository;
        private readonly IUserResolverService _userResolverService;
        private readonly IPaginationService _paginationService;
        private readonly IRootRepository<BookRootComment> _rootCommentRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHangfireJobScheduleService _hangfireJobScheduleService;
        private readonly IEmailSenderService _emailSenderService;

        public BookService(IRepository<Book> bookRepository, IMapper mapper, IRepository<BookAuthor> bookAuthorRepository, IRepository<BookGenre> bookGenreRepository,
            IRepository<Language> bookLanguageRepository, IRepository<User> userLocationRepository, IPaginationService paginationService, IRepository<Request> requestRepository,
            IUserResolverService userResolverService, IImageService imageService, IHangfireJobScheduleService hangfireJobScheduleService, IEmailSenderService emailSenderService, 
            IRootRepository<BookRootComment> rootCommentRepository)
        {
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookGenreRepository = bookGenreRepository;
            _bookLanguageRepository = bookLanguageRepository;
            _userLocationRepository = userLocationRepository;
            _requestRepository = requestRepository;
            _paginationService = paginationService;
            _mapper = mapper;
            _imageService = imageService;
            _userResolverService = userResolverService;
            _hangfireJobScheduleService = hangfireJobScheduleService;
            _emailSenderService = emailSenderService;
            _rootCommentRepository = rootCommentRepository;
        }

        public async Task<BookGetDto> GetByIdAsync(int bookId)
        {
            return _mapper.Map<BookGetDto>(await _bookRepository.GetAll()
                                                               .Include(p => p.BookAuthor)
                                                               .ThenInclude(x => x.Author)
                                                               .Include(p => p.BookGenre)
                                                               .ThenInclude(x => x.Genre)
                                                               .Include(x => x.Language)
                                                               .Include(p => p.User)
                                                               .ThenInclude(x => x.UserRoom)
                                                               .ThenInclude(x => x.Location)
                                                               .Include(p => p.Language)
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
            if (parameters.SortableParams != null)
            {
                query = query.OrderBy(parameters.SortableParams);
            }
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

        public async Task<bool> ActivateAsync(int bookId)
        {
            var book = _bookRepository.GetAll()
                .Include(i => i.User).Where(x => x.Id == bookId).ToList()
                .FirstOrDefault();
            if (book == null)
            {
                return false;
            }
            if (_userLocationRepository.FindByCondition(u => u.Email == book.User.Email).Result.IsEmailAllowed)
            {
                var emailMessageForBookActivated = new RequestMessage()
                {
                    UserName = book.User.FirstName + " " + book.User.LastName,
                    BookName = book.Name,
                    BookId = book.Id,
                    UserAddress = new MailboxAddress($"{book.User.Email}"),
                };
                await _emailSenderService.SendForBookActivatedAsync(emailMessageForBookActivated);
            }
            book.State = BookState.Available;
            await _bookRepository.Update(book, new List<string>() { "State" });
            await _bookRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(int bookId)
        {
            var book = _bookRepository.GetAll()
                .Include(i => i.User).Where(x => x.Id == bookId).ToList()
                .FirstOrDefault();
            if (book == null)
            {
                return false;
            }

            if (book.State == BookState.Requested)
            {
                var request = _requestRepository.GetAll()
                    .Include(i => i.Book)
                    .Include(i => i.Book)
                    .Include(i => i.User).Where(x=>x.BookId == bookId).ToList()
                    .Last();
                if (_userLocationRepository.FindByCondition(u => u.Email == book.User.Email).Result.IsEmailAllowed)
                {
                    var emailMessageForBookDeactivatedForRequester = new RequestMessage()
                    {
                        UserName = request.User.FirstName + " " + request.User.LastName,
                        BookName = book.Name,
                        BookId = book.Id,
                        UserAddress = new MailboxAddress($"{request.User.Email}"),
                    };
                    await _emailSenderService.SendForBookDeactivatedAsync(emailMessageForBookDeactivatedForRequester);
                }
                _hangfireJobScheduleService.DeleteRequestScheduleJob(request.Id);
                _requestRepository.Remove(request);
                await _requestRepository.SaveChangesAsync();
            }
            if (_userLocationRepository.FindByCondition(u => u.Email == book.User.Email).Result.IsEmailAllowed)
            {
                var emailMessageForBookDeactivatedForOwner = new RequestMessage()
                {
                    UserName = book.User.FirstName + " " + book.User.LastName,
                    BookName = book.Name,
                    BookId = book.Id,
                    UserAddress = new MailboxAddress($"{book.User.Email}"),
                };
                await _emailSenderService.SendForBookDeactivatedAsync(emailMessageForBookDeactivatedForOwner);
            }
            book.State = BookState.InActive;
            await _bookRepository.Update(book, new List<string>() { "State" });
            await _bookRepository.SaveChangesAsync();

            return true;
        }


        private IQueryable<Book> GetFilteredQuery(IQueryable<Book> query, BookQueryParams parameters)
        {
            if (parameters.ShowAvailable == true)
            {
                query = query.Where(b => b.State == BookState.Available);
            }
            if (parameters.Location != null)
            {
                query = query.Where(x => x.User.UserRoom.LocationId == parameters.Location);
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
            if (parameters.Languages != null)
            {
                var predicate = PredicateBuilder.New<Book>();
                foreach (var id in parameters.Languages)
                {
                    predicate = predicate.Or(g => g.Language.Id == id);
                }
                query = query.Where(predicate);
            }

            var userLocation = _userLocationRepository.GetAll();
            var author = _bookAuthorRepository.GetAll();
            var genre = _bookGenreRepository.GetAll();
            var language = _bookLanguageRepository.GetAll();
            var bookIds =
                from b in query
                join g in genre on b.Id equals g.BookId
                join l in language on b.Language.Id equals l.Id
                join u in userLocation on b.UserId equals u.Id
                select b.Id;

            return query.Where(x => bookIds.Contains(x.Id))
                .Include(p => p.BookAuthor)
                .ThenInclude(x => x.Author)
                .Include(p => p.BookGenre)
                .ThenInclude(x => x.Genre)
                .Include(x => x.Language)
                .Include(p => p.User)
                .ThenInclude(x => x.UserRoom)
                .ThenInclude(x => x.Location)
                .Include(x => x.Language);
        }

    }
}
