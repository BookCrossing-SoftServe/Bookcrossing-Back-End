using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult<GenreDto>> GetGenre(int id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<List<GenreDto>>> GetAllGenres()
        {
            return Ok(await _genreService.GetAll());
        }

        // PUT: api/Genre
        [HttpPut]
        public async Task<IActionResult> PutGenre(GenreDto genreDto)
        {
            var updated = await _genreService.Update(genreDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Genre
        [HttpPost]
        public async Task<ActionResult<GenreDto>> PostGenre([FromBody]GenreDto genreDto)
        {
            var insertedGenre = await _genreService.Add(genreDto);
            return CreatedAtAction("GetGenre", new { id = insertedGenre.Id }, insertedGenre);
        }

        // DELETE: api/Genre/id
        [ModelValidationFilter]
        [HttpDelete("{id:min(1)}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreService.Remove(id);
            if (genre == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}