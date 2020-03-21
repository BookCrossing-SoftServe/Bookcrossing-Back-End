using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Entities = Domain.Entities;


namespace Application.Services.Implementation
{
    public class Request : IRequest
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRepository<Entities.Book> _bookRepository;
        public Request(IRequestRepository requestRepository,IRepository<Entities.Book> bookRepository)
        {
            _requestRepository = requestRepository;
            _bookRepository = bookRepository;
        }

        public async Task Make(int userId, int bookId)
        {
            var book = await _bookRepository.FindByIdAsync(bookId);
            var request = new Domain.Entities.Request
            {
                BookId = book.Id,
                OwnerId = book.UserId,
                UserId = userId,
                RequestDate = DateTime.UtcNow
            };
            _requestRepository.Add(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Request>> Get(int bookId)
        {
            return await _requestRepository.GetAllBookBequests(bookId);
        }

        public async Task Approve(int requestId)
        {
            var request = await _requestRepository.FindByIdAsync(requestId);
            request.ReceiveDate = DateTime.UtcNow;
            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }
    }
}
