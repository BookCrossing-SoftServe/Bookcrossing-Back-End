using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly Application.Services.Interfaces.IAuthor _authorService;

        public AuthorsController(IAuthor authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _authorService.GetById(id);
            if (author == null)
                return NotFound();
            return author;
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(AuthorDto authorDto)
        {
            var author = new Author() {FirstName = authorDto.FirstName, LastName = authorDto.LastName, MiddleName = authorDto.MiddleName, Id = authorDto.Id};
            await _authorService.Update(author);
            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(AuthorDto authorDto)
        {
            var author = new Author() { FirstName = authorDto.FirstName, LastName = authorDto.LastName, MiddleName = authorDto.MiddleName };
            await _authorService.Add(author);
            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> DeleteAuthor(int id)
        {
            var author = await _authorService.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorService.Remove(author);

            return author;
        }
    }
}
