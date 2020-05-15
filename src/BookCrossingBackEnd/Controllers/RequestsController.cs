using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using BookCrossingBackEnd.Filters;
using Microsoft.AspNetCore.Authorization;

namespace BookCrossingBackEnd.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IUserResolverService _userResolverService;
        public RequestsController(IRequestService requestService, IUserResolverService userResolverService)
        {
            _requestService = requestService;
            _userResolverService = userResolverService;
        }

        [Route("{bookId:min(1)}")]
        [HttpPost]
        public async Task<ActionResult<RequestDto>> Make([FromRoute] int bookId)
        {
            var userId = _userResolverService.GetUserId();
            var request = await _requestService.MakeAsync(userId, bookId);

            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
        [HttpGet]
        public async Task<ActionResult<PaginationDto<RequestDto>>> GetByUser([FromQuery] BookQueryParams query)
        {
            var userId = _userResolverService.GetUserId();
            var requests = await _requestService.GetAsync(x => x.UserId == userId && x.ReceiveDate == null, query);
            if (requests == null)
            {
                return NotFound();
            }
            return Ok(requests);
        }


        [Route("{bookId:min(1)}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetByBook([FromRoute] int bookId, [FromQuery] RequestsQueryParams query = null)
        {
            if (query.First || query.Last)
            {
                var request = await _requestService.GetByBookAsync(x => x.BookId == bookId, query);
                if (request == null)
                {
                    return NotFound();
                }
                return Ok(request);
            }
            var requests = await _requestService.GetAllByBookAsync(x => x.BookId == bookId);
            if (requests == null)
            {
                return NotFound();
            }

            return Ok(requests);
        }

        [ModelValidationFilter]
        [Route("{requestId:min(1)}")]
        [HttpPut]
        public async Task<IActionResult> ApproveReceive([FromRoute] int requestId)
        {
            var updated = await _requestService.ApproveReceiveAsync(requestId);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }

        [ModelValidationFilter]
        [Route("{requestId:min(1)}")]
        [HttpDelete]
        public async Task<IActionResult> Remove([FromRoute] int requestId)
        {
            var removed = await _requestService.RemoveAsync(requestId);
            if (!removed)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
