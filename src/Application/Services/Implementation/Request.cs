using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services.Implementation
{
    public class Request : IRequest
    {
        private readonly IRequestRepository _requestRepository;
        public Request(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public void MakeRequest(int bookId, int userId)
        {
            var request = new Domain.Entities.Request
            {
                BookId = 1,
                UserId = userId,
                OwnerId = 1,
                RequestDate = DateTime.UtcNow

            };
            _requestRepository.AddAsync(request);
            _requestRepository.SaveChangesAsync();
        }

        public IEnumerable<Domain.Entities.Request> BookRequests(int bookId)
        {
            return _requestRepository.GetAllBookBequests(bookId);
        }

        public void ApplyRequest(int id)
        {
            _requestRepository.FindByIdAsync(id).Result.ReceiveDate = DateTime.UtcNow;
            _requestRepository.SaveChangesAsync();
        }
    }
}
