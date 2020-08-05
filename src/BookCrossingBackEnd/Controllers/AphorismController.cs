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
        private readonly ILogger _logger;

        public AphorismController(IAphorismService aphorismService, ILogger<AphorismController> logger)
        {
            _aphorismService = aphorismService;
            _logger = logger;
        }

        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult<AphorismDto>> GetAphorism(int id)
        {
            _logger.LogInformation("Getting aphorism {Id}", id);
            var aphorism = await _aphorismService.GetById(id);
            if (aphorism == null)
            {
                _logger.LogWarning("GetAphorism({Id}) NOT FOUND", id);
                return NotFound();
            }
            return Ok(aphorism);
        }
    }
}