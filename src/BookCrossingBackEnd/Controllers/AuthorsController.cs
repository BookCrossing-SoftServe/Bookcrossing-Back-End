using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Entities = Domain.Entities;
using Services= Application.Services;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly Services.Interfaces.IAuthor _authorService;

        public AuthorsController(IAuthor authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _authorService.GetById(id);
            if (author == null)
                return NotFound();
            return Ok(author);
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<List<AuthorDto>>> GetAllAuthor()
        {
            return Ok(await _authorService.GetAll());
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(AuthorDto authorDto)
        {
            authorDto.Id = 0; // Do we need 2 DTOs? one without Id and one with?
            await _authorService.Update(authorDto);
            return NoContent();
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(AuthorDto authorDto)
        {
            var insertedId = await _authorService.Add(authorDto);
            authorDto.Id = insertedId;
            return CreatedAtAction("GetAuthor", new { id = authorDto.Id }, authorDto);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AuthorDto>> DeleteAuthor(int id)
        {
            var author = await _authorService.Remove(id);
            if (author == null)
                return NotFound();
            return Ok(author);
        }
    }
}
