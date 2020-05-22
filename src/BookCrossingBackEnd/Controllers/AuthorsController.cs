using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Domain.RDBMS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
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
        // GET: api/Authors/Paginated?page=
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginationDto<AuthorDto>>> GetAuthors([FromQuery] FullPaginationQueryParams paginationQuery)
        {
            return Ok(await _authorService.GetAll(paginationQuery));
        }
        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult> GetAuthors([FromQuery] int[] ids)
        {
            return Ok(await _authorService.GetAll(ids));
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
        [Authorize(Roles = "Admin")]
        [HttpPut("merge")]
        public async Task<IActionResult> PutAuthor([FromBody]AuthorMergeDto authorDto)
        {
            var author = await _authorService.Merge(authorDto);
            if (author == null)
            {
                return BadRequest();
            }
            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutAuthor([FromBody]AuthorDto authorDto)
        {
            var success = await _authorService.Update(authorDto);
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
        [Authorize(Roles = "Admin")]
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
