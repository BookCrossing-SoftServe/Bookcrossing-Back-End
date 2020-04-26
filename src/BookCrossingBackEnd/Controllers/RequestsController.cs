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
    [Authorize]
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
            var request = await _requestService.Make(userId, bookId);

            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
        [HttpGet]
        public async Task<ActionResult<PaginationDto<RequestDto>>> GetByUser([FromQuery] FullPaginationQueryParams query)
        {
            var userId = _userResolverService.GetUserId();
            var requests = await _requestService.Get(x => x.UserId == userId, query);
            if (requests == null)
            {
                return NotFound();
            }
            return Ok(requests);
        }

        [Route("{bookId:min(1)}")]
        [HttpGet]
        public async Task<ActionResult<PaginationDto<RequestDto>>> GetByBook([FromRoute] int bookId, [FromQuery] FullPaginationQueryParams query)
        {
            var requests = await _requestService.Get(x => x.BookId == bookId, query);
            if (requests == null)
            {
                return NotFound();
            }
            return Ok(requests);
        }

        [ModelValidationFilter]
        [Route("{requestId:min(1)}")]
        [HttpPut]
        public async Task<ActionResult<RequestDto>> ApproveReceive([FromRoute] int requestId)
        {
            var updated = await _requestService.ApproveReceive(requestId);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }

        [ModelValidationFilter]
        [Route("{requestId:min(1)}")]
        [HttpDelete]
        public async Task<ActionResult<RequestDto>> Remove([FromRoute] int requestId)
        {
            var removed = await _requestService.Remove(requestId);
            if (!removed)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
