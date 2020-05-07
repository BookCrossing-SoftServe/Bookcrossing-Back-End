using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
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
        public async Task<ActionResult<PaginationDto<AuthorDto>>> GetAuthors([FromQuery] FullPaginationQueryParams fullPaginationQuery)
        {
            return Ok(await _authorService.GetAll(fullPaginationQuery));
        }

        // GET: api/Authors/"Tom"
        [HttpGet("{filter}")]
        public async Task<ActionResult> GetAuthor(string filter)
        {
            var authors = await _authorService.FilterAuthors(filter);
            if (authors == null)
            {
                return NotFound();
            }
            return Ok(authors);
        }


        // PUT: api/Authors
        [HttpPut]
        public async Task<IActionResult> PutAuthor([FromForm]AuthorDto authorDto, [FromQuery] int[] authors)
        {
            bool success;
            if (authors?.Length > 0)
            {
                success = await _authorService.Merge(authorDto, authors);
            }
            else
            {
                success = await _authorService.Update(authorDto);
            }
            if (!success)
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
