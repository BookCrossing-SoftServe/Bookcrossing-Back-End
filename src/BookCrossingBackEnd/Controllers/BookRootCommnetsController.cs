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

        // GET: api/BookRootCommants/5
        [HttpGet("{bookId}")]
        public async Task<ActionResult<List<RootDto>>> GetByBookId([FromRoute]int bookId)
        {
            var comments = await _rootBookCommentService.GetByBookId(bookId);
            return Ok(comments);
        }

        // GET: api/BookRootCommants
        [HttpGet]
        public async Task<ActionResult<List<RootDto>>> GetAll()
        {
            var comments = await _rootBookCommentService.GetAll();
            return Ok(comments);
        }

        // PUT: api/BookRootCommants
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<int>> Put([FromBody] RootUpdateDto updateDto)
        {
            if(updateDto.CommentOwnerId != _userResolverService.GetUserId())
            {
                return Forbid();
            }
            int number = await _rootBookCommentService.Update(updateDto);
            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }

        // POST: api/BookRootCommants
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> Post([FromBody] RootInsertDto insertDto)
        {
            insertDto.CommentOwnerId = _userResolverService.GetUserId();
            int number = await _rootBookCommentService.Add(insertDto);
            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }

        // DELETE: api/BookRootCommants/5
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<int>> Delete([FromBody]RootDeleteDto  deleteDto)
        {
            if (deleteDto.CommentOwnerId != _userResolverService.GetUserId() && !_userResolverService.IsUserAdmin())
            {
                return Forbid();
            }
            int number = await _rootBookCommentService.Remove(deleteDto.Id);
            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }
    }
}
