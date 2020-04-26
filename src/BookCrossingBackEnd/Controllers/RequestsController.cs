using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.QueryableExtension;
using BookCrossingBackEnd.Filters;
using Microsoft.AspNetCore.Authorization;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        //[Authorize]
        [Route("{bookId:min(1)}")]
        [HttpPost]
        public async Task<ActionResult<RequestDto>> Make([FromRoute] int bookId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase))?.Value);
            return await _requestService.Make(userId, bookId);
        }
        //[Authorize]
        [Route("{bookId:min(1)}")]
        [HttpGet]
        public async Task<ActionResult<PaginationDto<RequestDto>>> Get([FromRoute] int bookId, [FromQuery] FullPaginationQueryParams fullPaginationQuery)
        {
            return Ok(await _requestService.Get(bookId, fullPaginationQuery));
        }
        //[Authorize]
        [ModelValidationFilter]
        [Route("{requestId:min(1)}")]
        [HttpPut]
        public async Task<ActionResult<RequestDto>> Approve([FromRoute] int requestId)
        {
            var updated = await _requestService.Approve(requestId);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }
        //[Authorize]
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
