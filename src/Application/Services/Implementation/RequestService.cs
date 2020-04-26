using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Email;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Hangfire;
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

        public RequestService(IRepository<Request> requestRepository,IRepository<Book> bookRepository, IMapper mapper, 
            IEmailSenderService emailSenderService, IRepository<User> userRepository, IPaginationService paginationService,
            IHangfireJobScheduleService hangfireJobScheduleService)
        {
            _requestRepository = requestRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
            _useRepository = userRepository;
            _paginationService = paginationService;
            _hangfireJobScheduleService = hangfireJobScheduleService;
        }
        /// <inheritdoc />
        public async Task<RequestDto> Make(int userId, int bookId)
        {
            var book = await _bookRepository.GetAll().Include(x=> x.User).FirstOrDefaultAsync(x=> x.Id == bookId);
            var isNotAvailableForRequest = book == null || book.Available == false;

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
            book.Available = false;
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
                UserName = book.User.FirstName + " " + book.User.LastName,
                BookName = book.Name,
                BookId = book.Id,
                RequestId = request.Id,
                UserAddress = new MailboxAddress($"{book.User.Email}"),
            };
            _hangfireJobScheduleService.ScheduleRequestJob(emailMessageForReceiveConfirmation);

            return _mapper.Map<RequestDto>(request);
        }
        /// <inheritdoc />
        public async Task<PaginationDto<RequestDto>> Get(Expression<Func<Request, bool>> predicate, FullPaginationQueryParams parameters)
        {
            var query = _requestRepository.GetAll()
                .Include(i => i.Book).ThenInclude(i => i.BookAuthor).ThenInclude(i => i.Author)
                .Include(i => i.Book).ThenInclude(i => i.BookGenre).ThenInclude(i => i.Genre)
                .Include(i => i.Owner).ThenInclude(i => i.UserLocation).ThenInclude(i => i.Location)
                .Include(i => i.User).ThenInclude(i => i.UserLocation).ThenInclude(i => i.Location)
                .Where(predicate);
            var requests = await _paginationService.GetPageAsync<RequestDto, Request>(query, parameters);
            var isEmpty = !requests.Page.Any();
            return isEmpty ? null : requests;
        }
        /// <inheritdoc />
        public async Task<bool> ApproveReceive(int requestId)
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
        public async Task<bool> Remove(int requestId)
        {
            var request = await _requestRepository.GetAll()
                .Include(x => x.Book)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == requestId);
            if (request == null)
            {
                return false;
            }
            var emailMessage = new RequestMessage()
            {
                OwnerName = request.Owner.FirstName + " " + request.Owner.LastName,
                BookName = request.Book.Name,
                RequestId = request.Id,
                OwnerAddress = new MailboxAddress($"{request.Owner.Email}")
            };
            await _emailSenderService.SendForCanceledRequestAsync(emailMessage);
            var book = await _bookRepository.FindByIdAsync(request.BookId);
            book.Available = true;
            await _bookRepository.SaveChangesAsync();
            _requestRepository.Remove(request);

            var affectedRows = await _requestRepository.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
