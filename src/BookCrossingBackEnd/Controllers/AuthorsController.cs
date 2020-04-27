using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Authors/5
        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult> GetAuthor(int id)
        {
            var author = await _authorService.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }
        
        [HttpGet]
        public async Task<ActionResult<PaginationDto<AuthorDto>>> GetAuthors([FromQuery] QueryParameters query)
        {
            return Ok(await _authorService.GetAuthors(query));
        }
        // PUT: api/Authors
        [HttpPut]
        public async Task<IActionResult> PutAuthor(AuthorDto authorDto)
        {
            var updated = await _authorService.Update(authorDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(AuthorDto authorDto)
        {
            var author = await _authorService.Add(authorDto);
            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id:min(1)}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var removed = await _authorService.Remove(id);
            if (!removed)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
