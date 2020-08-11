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

        [HttpGet]
        public async Task<IActionResult> GetAphorismAsync()
        { 
            return Ok(await _aphorismService.GetAphorismAsync());
        }
    }
}
