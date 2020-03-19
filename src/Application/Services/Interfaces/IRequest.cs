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
        void MakeRequest(int userId, int bookId);
        IEnumerable<Request> BookRequests(int bookId);
    }
}
