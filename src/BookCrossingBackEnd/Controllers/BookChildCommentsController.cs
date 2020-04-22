using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookChildCommentsController : ControllerBase
    {
        private readonly IBookChildCommentService _childBookCommentService;
        private readonly IUserResolverService _userResolverService;
        public BookChildCommentsController(IBookChildCommentService childBookCommentService, IUserResolverService userResolverService)
        {
            _childBookCommentService = childBookCommentService;
            _userResolverService = userResolverService;
        }
       
        // PUT: api/BookChildCommants
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<int>> Put([FromBody] ChildUpdateDto updateDto)
        {
            if (updateDto.OwnerId != _userResolverService.GetUserId())
            {
                return Forbid();
            }
            int number = await _childBookCommentService.Update(updateDto);
            if (number == 0)
            {
                return NotFound(number);
            }
            return Ok(number);
        }

        // POST: api/BookChildCommants
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> Post([FromBody] ChildInsertDto insertDto)
        {
            if (insertDto.OwnerId != _userResolverService.GetUserId())
            {
                return Forbid();
            }
            int number = await _childBookCommentService.Add(insertDto);
            if (number == 0)
            {
                return BadRequest(number);
            }
            return Ok(number);
        }

        // DELETE: api/BookChildCommants
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<int>> Delete([FromBody] ChildDeleteDto deleteDto)
        {
            if (deleteDto.OwnerId != _userResolverService.GetUserId() && !_userResolverService.IsUserAdmin())
            {
                return Forbid();
            }
            int number = await _childBookCommentService.Remove(deleteDto.Ids);
            if (number == 0)
            {
                return NotFound(number);
            }
            return Ok(number);
        }
    }
}
