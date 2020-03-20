using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IRequest
    {
        Task MakeRequest(int userId, int bookId);
        Task<IEnumerable<Request>> BookRequests(int bookId);
        Task ApplyRequest(int id);
    }
}
