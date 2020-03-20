using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequest _requestService;
        public RequestController(IRequest requestService)
        {
            _requestService = requestService;
        }
        [Authorize]
        [HttpPost("{bookId}")]
        public async Task RequestBook(int bookId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase))?.Value);
            
            await _requestService.MakeRequest(userId, bookId);
        }
        [Authorize]
        [Route("{bookId}/All")]
        [HttpGet]
        public async Task<IEnumerable<Request>> All(int bookId)
        {
            return await _requestService.BookRequests(bookId);
        }
        [Authorize]
        [Route("{requestId}/Apply")]
        [HttpPut]
        public async Task Apply(int requestId)
        {
            await _requestService.ApplyRequest(requestId);
        }

    }
}
