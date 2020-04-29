using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAllBooksAsync()
        {
            return Ok(await _bookService.GetAll());
        }

        [HttpGet("registered")]
        public async Task<ActionResult<List<BookDto>>> GetRegisteredBooks()
        {
            return Ok(await _bookService.GetRegistered());
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook([FromRoute] int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBookAsync([FromBody] BookDto bookDto)
        {
            var insertedBook = await _bookService.Add(bookDto);
            return CreatedAtAction("GetBook", new { id = insertedBook.Id }, insertedBook);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookAsync([FromRoute] int id, [FromBody] BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            if (bookDto == null)
            {
                return BadRequest();
            }

            var isBookUpdated = await _bookService.Update(bookDto);
            if (!isBookUpdated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteBookAsync([FromRoute] int id)
        {
            var isBookRemoved = await _bookService.Remove(id);
            if (!isBookRemoved)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}