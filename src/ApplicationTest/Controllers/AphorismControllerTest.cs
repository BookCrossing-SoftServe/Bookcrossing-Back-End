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
    class AphorismControllerTest
    {
        private Mock<IAphorismService> _aphorismService;
        private AphorismController _aphorismController;
        private Mock<ILogger<AphorismController>> _logger;

        [OneTimeSetUp]
        public void Setup()
        {
            _aphorismService = new Mock<IAphorismService>();
            _aphorismController = new AphorismController(_aphorismService.Object, _logger.Object);
        }

        [Test]
        public async Task GetAphorismAsync_AphorismNotExist_ReturnsNotFoundResult()
        {
            _aphorismService.Setup(s => s.GetAphorismByIdAsync(It.IsAny<int>())).ReturnsAsync(null as AphorismDto);

            var result = await _aphorismController.GetAphorismByIdAsyc(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetAphorismAsync_AphorismExists_ReturnsOkObjectResultWithRequestedId()
        {
            var testAphorism = GetTestAphorism();
            _aphorismService.Setup(s => s.GetAphorismByIdAsync(It.IsAny<int>())).ReturnsAsync(testAphorism);

            var aphorismResult = await _aphorismController.GetAphorismByIdAsyc(It.IsAny<int>());

            var okResult = aphorismResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultAphorism = okResult.Value as AphorismDto;
            resultAphorism.Id.Should().Be(testAphorism.Id);
        }

        private AphorismDto GetTestAphorism()
        {
            return new AphorismDto() { Id = 1, Phrase = "Appreciate every moment", PhraseAuthor = "Ivan Franko" };
        }

        [Test]
        public async Task DeleteAphorism_AphorismExists_ReturnsOkResult()
        {
            _aphorismService.Setup(s => s.RemoveAphorismAsync((It.IsAny<int>()))).ReturnsAsync(true);

            var result = await _aphorismController.DeleteAphorismAsync(It.IsAny<int>());

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteAphorism_AphorismDoesNotExist_ReturnsNotFoundResult()
        {
            _aphorismService.Setup(s => s.RemoveAphorismAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _aphorismController.DeleteAphorismAsync(It.IsAny<int>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutAphorism_AphorismDoesNotExist_ReturnNotFound()
        {
            _aphorismService.Setup(s => s.UpdateAphorismAsync(It.IsAny<AphorismDto>())).ReturnsAsync(false);

            var result = await _aphorismController.PutAphorismAsync(It.IsAny<AphorismDto>());

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
