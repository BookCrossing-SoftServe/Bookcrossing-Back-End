using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
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

        public BookRootCommentsController(IBookRootCommentService rootBookCommentService)
        {
            _rootBookCommentService = rootBookCommentService;
        }

        // GET: api/BookRootCommants/5e965865bbcf5465603b9b9f
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<RootDto>> Get([FromRoute]string id)
        { 
            var comment = await _rootBookCommentService.GetById(id);

            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
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
        public async Task<ActionResult<int>> Put([FromBody] RootUpdateDto updateDto)
        {
            int number = await _rootBookCommentService.Update(updateDto);
            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }

        // POST: api/BookRootCommants
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] RootInsertDto insertDto)
        {
            int number = await _rootBookCommentService.Add(insertDto);

            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }

        // DELETE: api/BookRootCommants/5
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult<int>> Delete([FromRoute]string id)
        {
            int number = await _rootBookCommentService.Remove(id);

            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }
    }
}
