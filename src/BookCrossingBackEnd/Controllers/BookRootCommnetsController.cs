using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookRootCommentsController : ControllerBase
    {
        private readonly IBookRootCommentService _rootBookCommentService;
        private readonly IUserResolverService _userResolverService;
        public BookRootCommentsController(IBookRootCommentService rootBookCommentService, IUserResolverService userResolverService)
        {
            _rootBookCommentService = rootBookCommentService;
            _userResolverService = userResolverService;
        }

        // GET: api/RootBookCommants/5
        [HttpGet("{bookId}")]
        public async Task<ActionResult<List<RootDto>>> GetByBookId([FromRoute]int bookId)
        {
            var comments = await _rootBookCommentService.GetByBookId(bookId);
            return Ok(comments);
        }

        // GET: api/RootBookCommants
        [HttpGet]
        public async Task<ActionResult<List<RootDto>>> GetAll()
        {
            var comments = await _rootBookCommentService.GetAll();
            return Ok(comments);
        }

        // PUT: api/RootBookCommants
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<int>> Put([FromBody] RootUpdateDto updateDto)
        {

            if (updateDto.OwnerId != _userResolverService.GetUserId())

            {
                return Forbid();
            }
            int number = await _rootBookCommentService.Update(updateDto);
            if (number == 0)
            {
                return NotFound(number);
            }
            return Ok(number);
        }

        // POST: api/RootBookCommants
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> Post([FromBody] RootInsertDto insertDto)
        {
            if (insertDto.OwnerId != _userResolverService.GetUserId())
            {
                return Forbid();
            }
            int number = await _rootBookCommentService.Add(insertDto);
            if (number == 0)
            {
                return BadRequest(number);
            }
            return Ok(number);
        }

        // DELETE: api/RootBookCommants/5
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<int>> Delete([FromBody]RootDeleteDto deleteDto)
        {
            if (deleteDto.OwnerId != _userResolverService.GetUserId() && !_userResolverService.IsUserAdmin())
            {
                return Forbid();
            }
            int number = await _rootBookCommentService.Remove(deleteDto.Id);
            if (number == 0)
            {
                return NotFound(number);
            }
            return Ok(number);
        }
    }
}