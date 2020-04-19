using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookChildCommentsController : ControllerBase
    {
        private readonly IBookChildCommentService _childBookCommentService;

        public BookChildCommentsController(IBookChildCommentService childBookCommentService)
        {
            _childBookCommentService = childBookCommentService;
        }
       
        // PUT: api/BookChildCommants
        [HttpPut]
        public async Task<ActionResult<int>> Put([FromBody] ChildUpdateDto updateDto)
        {
            int number = await _childBookCommentService.Update(updateDto);
            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }

        // POST: api/BookChildCommants
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] ChildInsertDto insertDto)
        {
            int number = await _childBookCommentService.Add(insertDto);

            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }

        // DELETE: api/BookChildCommants
        [HttpDelete]
        public async Task<ActionResult<int>> Delete([FromBody] ChildDeleteDto deleteDto)
        {
            int number = await _childBookCommentService.Remove(deleteDto);

            if (number == 0)
            {
                return NotFound();
            }
            return Ok(number);
        }
    }
}
