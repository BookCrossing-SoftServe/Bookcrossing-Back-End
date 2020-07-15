using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.OuterSource;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OuterBooksSourceController : ControllerBase
    {
        private readonly IOuterBookSourceService _outerBookSourceService;

        public OuterBooksSourceController(IOuterBookSourceService outerBookSourceService)
        {
            _outerBookSourceService = outerBookSourceService;
        }

        [HttpGet("books")]
        public async Task<ActionResult<PaginationDto<OuterBookDto>>> Get([FromQuery]OuterSourceQueryParameters query)
        {
            var books = await _outerBookSourceService.SearchBooks(query);
            return Ok(books);
        }

        [HttpGet("book/{id}")]
        public async Task<ActionResult<OuterBookDto>> GetBook(int id)
        {
            var book = await _outerBookSourceService.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
    }
}
