using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    class LanguageControllerTest
    {
        private Mock<ILanguageService> _languageService;
        private LanguageController _languageController;
        private Mock<ILogger<LanguageController>> _logger;

        [OneTimeSetUp]
        public void Setup()
        {
            _languageService = new Mock<ILanguageService>();
            _logger = new Mock<ILogger<LanguageController>>();
            _languageController = new LanguageController(_languageService.Object, _logger.Object);
        }

        [Test]
        public async Task GetAlllLanguagesAsync_Success_ReturnsOkObjectResult()
        {
            var testLanguages = GetTestLanguages();
            _languageService.Setup(s => s.GetAll()).ReturnsAsync(testLanguages);

            var result = await _languageController.GetAllLanguages();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
        }

        private List<LanguageDto> GetTestLanguages()
        {
            return new List<LanguageDto>
            {
                new LanguageDto(),
                new LanguageDto(),
                new LanguageDto()
            };
        }

        private LanguageDto GetTestLanguage()
        {
            return new LanguageDto() { Id = 1, Name = "Language" };
        }

        [Test]
        public async Task GetLanguageAsync_LanguageNotExist_ReturnsNotFoundResult()
        {
          _languageService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(null as LanguageDto);

            var result = await _languageController.GetLanguage(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteLanguage_LanguageExists_Returns_OkResult()
        {
            _languageService.Setup(s => s.Remove((It.IsAny<int>()))).ReturnsAsync(true);

            var result = await _languageController.DeleteLanguage(It.IsAny<int>());

            result.Should().BeOfType<OkResult>();
        }
    }
}
