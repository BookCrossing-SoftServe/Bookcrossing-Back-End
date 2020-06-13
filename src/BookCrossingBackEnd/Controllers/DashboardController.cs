using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Domain.RDBMS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: api/Dashboard
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string city)
        {
            return Ok(await _dashboardService.GetAll(city));
        }

        // GET: api/Dashboard/Location
        [HttpGet("Location")]
        public async Task<ActionResult> GetLocationData([FromQuery] string city)
        {
            var locationData = await _dashboardService.GetLocationData(city);
            return Ok(locationData);
        }

        // GET: api/Dashboard/Availability
        [HttpGet("Availability")]
        public async Task<ActionResult> GetAvailability([FromQuery] string city)
        {
            var availabilityData = await _dashboardService.GetAvailabilityData(city);
            return Ok(availabilityData);
        }

        // GET: api/Dashboard/BookUserComparison
        [HttpGet("BookUserComparison")]
        public async Task<ActionResult> GetBookUserData([FromQuery] string city, [FromQuery] bool byMonth = true)
        {
            var bookUserComparisonData = await _dashboardService.GetBookUserData(city,byMonth);
            return Ok(bookUserComparisonData);
        }
    }
}
