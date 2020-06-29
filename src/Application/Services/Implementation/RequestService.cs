using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using Hangfire;
using LinqKit;
using MailKit;
using Microsoft.EntityFrameworkCore;
using MimeKit;


namespace Application.Services.Implementation
{
    public class RequestService : IRequestService
    {
        private readonly IRepository<Request> _requestRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IRepository<User> _useRepository;
        private readonly IPaginationService _paginationService;
        private readonly IHangfireJobScheduleService _hangfireJobScheduleService;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        private readonly IRepository<Language> _bookLanguageRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<UserRoom> _userLocationRepository;
        private readonly IRootRepository<BookRootComment> _rootCommentRepository;


        public RequestService(IRepository<Request> requestRepository,IRepository<Book> bookRepository, IMapper mapper, 
            IEmailSenderService emailSenderService, IRepository<User> userRepository, IPaginationService paginationService,
            IRepository<Language> bookLanguageRepository, IHangfireJobScheduleService hangfireJobScheduleService, IRepository<BookAuthor> bookAuthorRepository, 
            IRepository<BookGenre> bookGenreRepository, IRepository<UserRoom> userLocationRepository, IRootRepository<BookRootComment> rootCommentRepository)
        {
            _requestRepository = requestRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
            _useRepository = userRepository;
            _paginationService = paginationService;
            _hangfireJobScheduleService = hangfireJobScheduleService;
            _bookGenreRepository = bookGenreRepository;
            _bookLanguageRepository = bookLanguageRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _userLocationRepository = userLocationRepository;
            _rootCommentRepository = rootCommentRepository;
        }
        /// <inheritdoc />
        public async Task<RequestDto> MakeAsync(int userId, int bookId)
        {
            var book = await _bookRepository.GetAll().Include(x=> x.User).FirstOrDefaultAsync(x=> x.Id == bookId);
            var isNotAvailableForRequest = book == null || book.State != BookState.Available;

            if (isNotAvailableForRequest)
            {
                return null;
            }

            var request = new Request()
            {
                BookId = book.Id,
                OwnerId = book.UserId,
                UserId = userId,
                RequestDate = DateTime.UtcNow
            };
            _requestRepository.Add(request);
            await _requestRepository.SaveChangesAsync();
            book.State = BookState.Requested;
            await _bookRepository.SaveChangesAsync();
            var user = _useRepository.FindByIdAsync(userId).Result;
            var emailMessageForRequest = new RequestMessage()
            {
                OwnerName = book.User.FirstName + " " + book.User.LastName, BookName = book.Name,
                RequestDate = request.RequestDate, RequestId = request.Id, OwnerAddress = new MailboxAddress($"{book.User.Email}"),
                UserName = user.FirstName + " " + user.LastName
            };
            await _emailSenderService.SendForRequestAsync(emailMessageForRequest);

            var emailMessageForReceiveConfirmation = new RequestMessage()
            {
                UserName = user.FirstName + " " + user.LastName,
                BookName = book.Name,
                BookId = book.Id,
                RequestId = request.Id,
                UserAddress = new MailboxAddress($"{user.Email}"),
            };
            _hangfireJobScheduleService.ScheduleRequestJob(emailMessageForReceiveConfirmation);

            return _mapper.Map<RequestDto>(request);
        }
        /// <inheritdoc />
        public async Task<RequestDto> GetByBookAsync(Expression<Func<Request, bool>> predicate, RequestsQueryParams query)
        {
            Request request = null;
            if (query.First)
            {
                request = await _requestRepository.GetAll()
                    .Include(i => i.Book).ThenInclude(i => i.BookAuthor).ThenInclude(i => i.Author)
                    .Include(i => i.Book).ThenInclude(i => i.BookGenre).ThenInclude(i => i.Genre)
                    .Include(i => i.Book).ThenInclude(i => i.Language).ThenInclude(i => i.Name)
                    .Include(i => i.Owner).ThenInclude(i => i.UserRoom).ThenInclude(i => i.Location)
                    .Include(i => i.User).ThenInclude(i => i.UserRoom).ThenInclude(i => i.Location)
                    .FirstOrDefaultAsync(predicate);
            }
            else if(query.Last)
            {
                request = _requestRepository.GetAll()
                    .Include(i => i.Book).ThenInclude(i => i.BookAuthor).ThenInclude(i => i.Author)
                    .Include(i => i.Book).ThenInclude(i => i.BookGenre).ThenInclude(i => i.Genre)
                    .Include(i => i.Book).ThenInclude(i => i.Language).ThenInclude(i => i.Name)
                    .Include(i => i.Owner).ThenInclude(i => i.UserRoom).ThenInclude(i => i.Location)
                    .Include(i => i.User).ThenInclude(i => i.UserRoom).ThenInclude(i => i.Location).Where(predicate).ToList()
                    .Last();
            }
            if (request == null)
            {
                return null;
            }
            return _mapper.Map<RequestDto>(request);
        }
        /// <inheritdoc />
        public async Task<IEnumerable<RequestDto>> GetAllByBookAsync(Expression<Func<Request, bool>> predicate)
        {

            var requests = _requestRepository.GetAll()
                .Include(i => i.Book).ThenInclude(i => i.BookAuthor).ThenInclude(i => i.Author)
                .Include(i => i.Book).ThenInclude(i => i.BookGenre).ThenInclude(i => i.Genre)
                .Include(i => i.Book).ThenInclude(i => i.Language).ThenInclude(i => i.Name)
                .Include(i => i.Owner).ThenInclude(i => i.UserRoom).ThenInclude(i => i.Location)
                .Include(i => i.User).ThenInclude(i => i.UserRoom).ThenInclude(i => i.Location)
                .Where(predicate);
            if(requests == null)
            {
                return null;
            }
            return _mapper.Map<List<RequestDto>>(requests);
        }
        /// <inheritdoc />
        public async Task<PaginationDto<RequestDto>> GetAsync(Expression<Func<Request, bool>> predicate, BookQueryParams parameters)
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
                        a.Author.FirstName.Contains(term[0]) && a.Author.LastName.Contains(term[term.Length - 1]) || a.Book.Name.Contains(parameters.SearchTerm));
                }
            }

            var genre = _bookGenreRepository.GetAll();
            if (parameters.Genres != null)
            {
                var wherePredicate = PredicateBuilder.New<BookGenre>();
                foreach (var id in parameters.Genres)
                {
                    var tempId = id;
                    wherePredicate = wherePredicate.Or(g => g.Genre.Id == tempId);
                }
                genre = genre.Where(wherePredicate);
            }

            var lang = _bookLanguageRepository.GetAll();
            if (parameters.Languages != null)
            {
                var wherePredicate = PredicateBuilder.New<Language>();
                foreach (var id in parameters.Languages)
                {
                    var tempId = id;
                    wherePredicate = wherePredicate.Or(g => g.Id == tempId);
                }
                lang = lang.Where(wherePredicate);
            }

            if (parameters.ShowAvailable == true)
            {
                books = books.Where(b => b.State == BookState.Available);
            }

            var location = _userLocationRepository.GetAll();
            if (parameters.Location != null)
            {
                location = location.Where(l => l.Location.Id == parameters.Location);
            }
            var bookIds =
                from b in books
                join g in genre on b.Id equals g.BookId
                join la in lang on b.Language.Id equals la.Id
                join a in author on b.Id equals a.BookId
                join l in location on b.UserId equals l.Id
                select b.Id;
            var query = _requestRepository.GetAll()
                .Include(i => i.Book).ThenInclude(i => i.BookAuthor).ThenInclude(i => i.Author)
                .Include(i => i.Book).ThenInclude(i => i.BookGenre).ThenInclude(i => i.Genre)
                .Include(i => i.Book).ThenInclude(i => i.Language).ThenInclude(i => i.Name)
                .Include(i => i.Owner).ThenInclude(i => i.UserRoom).ThenInclude(i => i.Location)
                .Include(i => i.User).ThenInclude(i => i.UserRoom).ThenInclude(i => i.Location)
                .Where(predicate).Where(x => bookIds.Contains(x.BookId));

            var requests =  await _paginationService.GetPageAsync<RequestDto, Request>(query, parameters);
            var isEmpty = !requests.Page.Any();
            return isEmpty ? null : requests;
        }

        /// <inheritdoc />
        public async Task<bool> ApproveReceiveAsync(int requestId)
        {
            var request = await _requestRepository.GetAll()
                .Include(x=>x.Book)
                .Include(x=>x.User)
                .FirstOrDefaultAsync(x => x.Id == requestId);
            if (request == null)
            {
                return false;
            }
            var book = await _bookRepository.FindByIdAsync(request.BookId);
            book.User = request.User;
            book.State = BookState.Reading;
            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();
            request.ReceiveDate = DateTime.UtcNow;
            _requestRepository.Update(request);
            var affectedRows = await _requestRepository.SaveChangesAsync();
            var emailMessage = new RequestMessage()
            {
                OwnerName = request.User.FirstName + " " + request.User.LastName,
                BookName = request.Book.Name,
                RequestId = request.Id,
                OwnerAddress = new MailboxAddress($"{request.User.Email}")
            };
            _hangfireJobScheduleService.DeleteRequestScheduleJob(requestId);
            await _emailSenderService.SendThatBookWasReceivedAsync(emailMessage);
            return affectedRows > 0;
        }
        /// <inheritdoc />
        public async Task<bool> RemoveAsync(int requestId)
        {
            var request = await _requestRepository.GetAll()
                .Include(x => x.Book)
                .Include(x => x.Owner)
                .Include(x=>x.User)
                .FirstOrDefaultAsync(x => x.Id == requestId);
            if (request == null)
            {
                return false;
            }
            var emailMessage = new RequestMessage()
            {
                UserName = request.User.FirstName + " " + request.User.LastName,
                OwnerName = request.Owner.FirstName + " " + request.Owner.LastName,
                BookName = request.Book.Name,
                RequestId = request.Id,
                OwnerAddress = new MailboxAddress($"{request.Owner.Email}")
            };
            _hangfireJobScheduleService.DeleteRequestScheduleJob(requestId);
            await _emailSenderService.SendForCanceledRequestAsync(emailMessage);
            var book = await _bookRepository.FindByIdAsync(request.BookId);
            book.State = BookState.Available;
            await _bookRepository.SaveChangesAsync();
            _requestRepository.Remove(request);

            var affectedRows = await _requestRepository.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
