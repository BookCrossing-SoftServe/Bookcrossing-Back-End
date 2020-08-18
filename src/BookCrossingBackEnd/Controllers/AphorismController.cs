using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AphorismController : ControllerBase
    {
        private readonly IAphorismService _aphorismService;

        public AphorismController(IAphorismService aphorismService)
        {
            _aphorismService = aphorismService;
        }

        [HttpGet("{current}")]
        public async Task<IActionResult> GetAphorismAsync(bool current)
        {
            return Ok(await _aphorismService.GetCurrentAphorismAsync(current));
        }

        [HttpGet]
        public async Task<ActionResult<List<AphorismDto>>> GetAllAphorisms()
        {
            return Ok(await _aphorismService.GetAllAphorismsAsync());
        }

        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult<AphorismDto>> GetAphorismByIdAsyc(int id)
        {
            var aphorism = await _aphorismService.GetAphorismByIdAsync(id);
            if (aphorism == null)
            {
                return NotFound();
            }
            return Ok(aphorism);
        }

        // PUT: api/Aphorism
        [HttpPut]
        public async Task<IActionResult> PutAphorismAsync(AphorismDto aphorismDto)
        {
            var updated = await _aphorismService.UpdateAphorismAsync(aphorismDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Aphorism
        [HttpPost]
        public async Task<ActionResult<AphorismDto>> PostAphorismAsync([FromBody]AphorismDto aphorismDto)
        {
            var insertedAphorism = await _aphorismService.AddAphorismAsync(aphorismDto);
            return Created("GetAphorism", insertedAphorism);
        }

        // DELETE: api/Aphorism/id
        [HttpDelete("{id:min(1)}")]
        public async Task<IActionResult> DeleteAphorismAsync(int id)
        {
            var aphorism = await _aphorismService.RemoveAphorismAsync(id);
            if (aphorism == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
