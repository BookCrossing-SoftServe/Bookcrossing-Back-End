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
using AutoMapper;
using Domain.Entities;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Entities = Domain.Entities;


namespace Application.Services.Implementation
{
    public class Request : IRequest
    {
        private readonly IRepository<Entities.Request> _requestRepository;
        private readonly IRepository<Entities.Book> _bookRepository;
        private readonly IMapper _mapper;
        public Request(IRepository<Entities.Request> requestRepository,IRepository<Entities.Book> bookRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        /// <inheritdoc />
        public async Task<RequestDto> Make(int userId, int bookId)
        {
            var book = await _bookRepository.FindByIdAsync(bookId);
            var request = new Entities.Request()
            {
                BookId = book.Id,
                OwnerId = book.UserId,
                UserId = userId,
                RequestDate = DateTime.UtcNow
            };
            _requestRepository.Add(request);
            await _requestRepository.SaveChangesAsync();
            return _mapper.Map<RequestDto>(request);
        }
        /// <inheritdoc />
        public IEnumerable<RequestDto> Get(int bookId)
        {
            return _mapper.Map<IEnumerable<RequestDto>>(_requestRepository.GetAll().Where(i => i.BookId == bookId));
        }
        /// <inheritdoc />
        public async Task<RequestDto> Approve(int requestId)
        {
            var request = await _requestRepository.FindByIdAsync(requestId);
            request.ReceiveDate = DateTime.UtcNow;
            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
            return _mapper.Map<RequestDto>(request);
        }
        /// <inheritdoc />
        public async Task<RequestDto> Remove(int requestId)
        {
            var request = await _requestRepository.FindByIdAsync(requestId);
            if (request == null)
                return null;
            _requestRepository.Remove(request);
            await _requestRepository.SaveChangesAsync();
            return _mapper.Map<RequestDto>(request);
        }
    }
}
