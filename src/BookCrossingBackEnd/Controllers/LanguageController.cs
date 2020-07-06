using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;

        public LanguageController(ILanguageService languageService, ILogger<LanguageController> logger)
        {
            _languageService = languageService;
            _logger = logger;
        }

        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult<LanguageDto>> GetLanguage(int id)
        {
            _logger.LogInformation("Getting language {Id}", id);
            var language = await _languageService.GetById(id);
            if (language == null)
            {
                _logger.LogWarning("GetLanguage({Id}) NOT FOUND", id);
                return NotFound();
            }
            return Ok(language);
        }

        // GET: api/Languages
        [HttpGet]
        public async Task<ActionResult<List<LanguageDto>>> GetAllLanguages()
        {
            _logger.LogInformation("Getting all languages");
            return Ok(await _languageService.GetAll());
        }

        // PUT: api/Language
        [HttpPut]
        public async Task<IActionResult> PutLanguage(LanguageDto languageDto)
        {
            _logger.LogInformation("Update language {LanguageDto}", languageDto);
            var updated = await _languageService.Update(languageDto);
            if (!updated)
            {
                _logger.LogWarning("Update language ({LanguageDto}) NOT FOUND", languageDto);
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Language
        [HttpPost]
        public async Task<ActionResult<LanguageDto>> PostLanguage([FromBody]LanguageDto languageDto)
        {
            _logger.LogInformation("Post language {LanguageDto}", languageDto);
            var insertedLanguage = await _languageService.Add(languageDto);
            return Created("GetLanguage", insertedLanguage);
        }

        // DELETE: api/Language/id
        [HttpDelete("{id:min(1)}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            _logger.LogInformation("Delete language {Id}", id);
            var language = await _languageService.Remove(id);
            if (language == false)
            {
                _logger.LogWarning("Delete language ({Id}) NOT FOUND", id);
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<PaginationDto<LanguageDto>>> GetAllGenres([FromQuery] FullPaginationQueryParams fullPaginationQuery)
        {
            _logger.LogInformation("Getting all paginated languages");
            return Ok(await _languageService.GetAll(fullPaginationQuery));
        }
    }
}