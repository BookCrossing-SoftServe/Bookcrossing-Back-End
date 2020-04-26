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
using Domain.RDBMS;
using Domain.RDBMS.Entities;
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

        public RequestService(IRepository<Request> requestRepository,IRepository<Book> bookRepository, IMapper mapper, 
            IEmailSenderService emailSenderService, IRepository<User> userRepository, IPaginationService paginationService)
        {
            _requestRepository = requestRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
            _useRepository = userRepository;
            _paginationService = paginationService;
        }
        /// <inheritdoc />
        public async Task<RequestDto> Make(int userId, int bookId)
        {
            var book = await _bookRepository.GetAll().Include(x=> x.User).FirstOrDefaultAsync(x=> x.Id == bookId);

            var request = new Request()
            {
                BookId = book.Id,
                OwnerId = book.UserId,
                UserId = userId,
                RequestDate = DateTime.UtcNow
            };
            _requestRepository.Add(request);
            await _requestRepository.SaveChangesAsync();
            var user = _useRepository.FindByIdAsync(userId).Result;
            var emailMessage = new RequestMessage()
            {
                UserName = book.User.FirstName + " " + book.User.LastName, BookName = book.Name,
                BookId = book.Id,
                RequestDate = request.RequestDate, RequestNumber = request.Id, UserEmail = new MailboxAddress($"{book.User.Email}"),
                RequestedUser = user.FirstName + " " + user.LastName, Subject = $"Request for {book.Name}"
            };
            await _emailSenderService.SendEmailForRequestAsync(emailMessage);
            return _mapper.Map<RequestDto>(request);
        }
        /// <inheritdoc />
        public async Task<PaginationDto<RequestDto>> Get(int bookId, FullPaginationQueryParams parameters)
        {
            var query = _requestRepository.GetAll()
                .Include(i => i.Book).ThenInclude(i => i.BookAuthor).ThenInclude(i => i.Author)
                .Include(i => i.Book).ThenInclude(i => i.BookGenre).ThenInclude(i => i.Genre)
                .Include(i => i.Owner).ThenInclude(i => i.UserLocation).ThenInclude(i => i.Location)
                .Include(i => i.User).ThenInclude(i => i.UserLocation).ThenInclude(i => i.Location)
                .Where(i => i.BookId == bookId);
            return await _paginationService.GetPageAsync<RequestDto, Request>(query, parameters);
        }
        /// <inheritdoc />
        public async Task<bool> Approve(int requestId)
        {
            var request = await _requestRepository.FindByIdAsync(requestId);
            if (request == null)
            {
                return false;
            }
            request.ReceiveDate = DateTime.UtcNow;
            _requestRepository.Update(request);
            var affectedRows = await _requestRepository.SaveChangesAsync();
            return affectedRows > 0;
        }
        /// <inheritdoc />
        public async Task<bool> Remove(int requestId)
        {
            var request = await _requestRepository.FindByIdAsync(requestId);
            if (request == null)
            {
                return false;
            }
            _requestRepository.Remove(request);
            var affectedRows = await _requestRepository.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
