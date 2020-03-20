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
        [HttpPost]
        public async Task Make(int bookId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase))?.Value);
            
            await _requestService.Make(userId, bookId);
        }
        [Authorize]
        [Route("byBook/{bookId}")]
        [HttpGet]
        public async Task<IEnumerable<Request>> Get(int bookId)
        {
            return await _requestService.Get(bookId);
        }
        [Authorize]
        [Route("{requestId}")]
        [HttpPut]
        public async Task Approve(int requestId)
        {
            await _requestService.Approve(requestId);
        }

    }
}
