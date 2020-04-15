using Application.Dto.Comment;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookCrossingBackEnd.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookCommentsController : ControllerBase
    {
        private readonly IBookCommentService _bookCommentService;

        public BookCommentsController(IBookCommentService bookCommentService)
        {
            _bookCommentService = bookCommentService;
        }

        // GET: api/Commants/5e965865bbcf5465603b9b9f
        [HttpGet("{CommentId:length(24)}")]
        public async Task<ActionResult<BookRootCommentDto>> Get([FromRoute]string CommentId)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            var comment = await _bookCommentService.GetById(CommentId);

            var resultTime = startTime.Elapsed;
            string executingTime = string.Format(" {0:00}:{1:00}:{2:00}.{3:000}",
                resultTime.Hours,
                resultTime.Minutes,
                resultTime.Seconds,
                resultTime.Milliseconds);
            return Ok(new { ExecutingTime = executingTime, Comment = comment });
        }

        // GET: api/Commants/5
        [HttpGet("{bookId:int}")]
        public async Task<ActionResult<List<BookRootCommentDto>>> GetByBookId([FromRoute]int bookId)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            var comments = await _bookCommentService.GetByBookId(bookId);

            var resultTime = startTime.Elapsed;
            string executingTime = string.Format(" {0:00}:{1:00}:{2:00}.{3:000}",
                resultTime.Hours,
                resultTime.Minutes,
                resultTime.Seconds,
                resultTime.Milliseconds);
            return Ok(new { ExecutingTime = executingTime, Comments = comments });
        }

        // GET: api/Commants
        [HttpGet]
        public async Task<ActionResult<List<BookRootCommentDto>>> GetAll()
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            var comments = await _bookCommentService.GetAll();

            var resultTime = startTime.Elapsed;
            string executingTime = string.Format(" {0:00}:{1:00}:{2:00}.{3:000}",
                resultTime.Hours,
                resultTime.Minutes,
                resultTime.Seconds,
                resultTime.Milliseconds);
            return Ok(new { ExecutingTime = executingTime, Comments = comments });
        }

        // PUT: api/Commants
        [HttpPut]
        public async Task<ActionResult<int>> Put([FromBody] BookCommentUpdateDto updateDto)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            int number = await _bookCommentService.Update(updateDto);

            var resultTime = startTime.Elapsed;
            string executingTime = string.Format(" {0:00}:{1:00}:{2:00}.{3:000}",
                resultTime.Hours,
                resultTime.Minutes,
                resultTime.Seconds,
                resultTime.Milliseconds);
            return Ok(new { ExecutingTime = executingTime, ModifModifiedCount = number });
        }

        // POST: api/Commants
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] BookCommentInsertDto insertDto)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            int number = await _bookCommentService.Add(insertDto);

            var resultTime = startTime.Elapsed;
            string executingTime = string.Format(" {0:00}:{1:00}:{2:00}.{3:000}",
                resultTime.Hours,
                resultTime.Minutes,
                resultTime.Seconds,
                resultTime.Milliseconds);
            return Ok(new { ExecutingTime = executingTime, InsertedNumber = number });
        }

        // DELETE: api/Commants
        [HttpDelete]
        public async Task<ActionResult<int>> Delete([FromBody] BookCommentDeleteDto deleteDto)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            int number = await _bookCommentService.Remove(deleteDto);

            var resultTime = startTime.Elapsed;
            string executingTime = string.Format(" {0:00}:{1:00}:{2:00}.{3:000}",
                resultTime.Hours,
                resultTime.Minutes,
                resultTime.Seconds,
                resultTime.Milliseconds);
            return Ok(new { ExecutingTime = executingTime, DeletedNumber = number });
        }
    }
}
