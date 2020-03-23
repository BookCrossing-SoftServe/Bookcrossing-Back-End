using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequest _requestService;
        public RequestsController(IRequest requestService)
        {
            _requestService = requestService;
        }
        [Authorize]
        [Route("{bookId}")]
        [HttpPost]
        public async Task<ActionResult<RequestDto>> Make([FromRoute] int bookId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase))?.Value);
            return await _requestService.Make(userId, bookId);
        }
        [Authorize]
        [Route("{bookId}")]
        [HttpGet]
        public ActionResult<IEnumerable<Request>> Get([FromRoute] int bookId)
        {
            var requests = _requestService.Get(bookId);
            if (requests == null)
                return NotFound();
            return Ok(requests);
        }
        [Authorize]
        [Route("{requestId}")]
        [HttpPut]
        public async Task<ActionResult<RequestDto>> Approve([FromRoute] int requestId)
        {
            var request = await _requestService.Approve(requestId);
            if (request == null)
                return NotFound();
            return Ok(request);
        }
        [Authorize]
        [Route("{requestId}")]
        [HttpDelete]
        public async Task<ActionResult<RequestDto>> Remove([FromRoute] int requestId)
        {
            var request = await _requestService.Remove(requestId);
            if (request == null)
                return NotFound();
            return Ok(request);
        }

    }
}
