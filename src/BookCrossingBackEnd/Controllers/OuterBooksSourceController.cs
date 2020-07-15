using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto.QueryParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/outerservice")]
    [ApiController]
    public class OuterBooksSourceController : ControllerBase
    {
        private readonly IOuterBookSourceService _outerBookSourceService;

        public OuterBooksSourceController(IOuterBookSourceService outerBookSourceService)
        {
            _outerBookSourceService = outerBookSourceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]OuterSourceQueryParameters query)
        {
            var books = await _outerBookSourceService.SearchBooks(query);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _outerBookSourceService.GetBook(id);
            return Ok(book);
        }
    }
}
