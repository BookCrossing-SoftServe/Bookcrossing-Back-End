using System;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationDto<BookGetDto>>> GetCurrentUserWishList([FromQuery] PageableParams pageableParams)
        {
            return await _wishListService.GetWishesOfCurrentUser(pageableParams);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddWish([FromBody]int bookId)
        {
            try
            {
                await _wishListService.AddWish(bookId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteWish(int bookId)
        {
            await _wishListService.RemoveWish(bookId);
            return Ok();
        }
    }
}
