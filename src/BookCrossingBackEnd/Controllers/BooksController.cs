using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBook _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBook bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<BookDto>> GetAllBooks()
        {
            //logger part

            //_logger.LogDebug("Debug");
            //_logger.LogInformation("GetAllBooks Information");
           // _logger.LogWarning("Warning");
           /* _logger.LogError("Error");
            _logger.LogCritical("Critical");*/

            /*try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }*/

            return Ok(await _bookService.GetAll());
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
        public async Task<ActionResult<BookDto>> PostBook([FromBody] BookDto bookDto)
        {
            var insertedId = await _bookService.Add(bookDto);
            bookDto.Id = insertedId;
            return CreatedAtAction("GetBook", new { id = bookDto.Id }, bookDto);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook([FromRoute] int id, [FromBody] BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }
            await _bookService.Update(bookDto);
            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookDto>> DeleteAuthor([FromRoute] int id)
        {
            var book = await _bookService.Remove(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

    }
}