using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    class GenreControllerTest
    {
        private Mock<IGenreService> _genreService;
        private GenreController _genreController;

        [OneTimeSetUp]
        public void Setup()
        {
            _genreService = new Mock<IGenreService>();
            _genreController = new GenreController(_genreService.Object);
        }

        [Test]
        public async Task GetAllGenresAsync_Returns_OkObjectResultWithRequestedCount()
        {
            var testGenres = GetTestGenres();
            _genreService.Setup(s => s.GetAll()).ReturnsAsync(testGenres);

            var result = await _genreController.GetAllGenres();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var genres = okResult.Value as List<GenreDto>;
            genres.Count().Should().Be(testGenres.Count);
        }

        List<GenreDto> GetTestGenres()
        {
            return new List<GenreDto>
            {
                new GenreDto(),
                new GenreDto(),
                new GenreDto()
            };
        }

        [Test]
        public async Task GetGenreAsync_GenreExists_Returns_OkObjectResultWithRequestedId()
        {
            var testGenre = GetTestGenre();
            _genreService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(testGenre);

            var genreResult = await _genreController.GetGenre(It.IsAny<int>());

            var okResult = genreResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultGenre = okResult.Value as GenreDto;
            resultGenre.Id.Should().Be(testGenre.Id);
        }

        private GenreDto GetTestGenre()
        {
            return new GenreDto() { Id = 1,Name = "Genre" };
        }

        [Test]
        public async Task GetGenreAsync_GenreDoesNotExist_Returns_NotFoundResult()
        {
            _genreService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(null as GenreDto);

            var result = await _genreController.GetGenre(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutGenre_GenreExists_Returns_NoContent()
        {
            _genreService.Setup(s => s.Update(It.IsAny<GenreDto>())).ReturnsAsync(true);

            var result = await _genreController.PutGenre(It.IsAny<GenreDto>());

            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutGenre_GenreDoesNotExist_Return_NotFound()
        {
            _genreService.Setup(s => s.Update(It.IsAny<GenreDto>())).ReturnsAsync(false);

            var result = await _genreController.PutGenre(It.IsAny<GenreDto>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PostGenre_Returns_CreatedAtActionResult()
        {
            var testGenre = GetTestGenre();
            _genreService.Setup(m => m.Add(It.IsAny<GenreDto>())).ReturnsAsync(testGenre);

            var createdAtActionResult = await _genreController.PostGenre(It.IsAny<GenreDto>());
            var result = (GenreDto)((CreatedAtActionResult)createdAtActionResult.Result).Value;

            result.Should().BeOfType<GenreDto>();
            createdAtActionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            result.Should().BeEquivalentTo(testGenre, options => options.Excluding(a => a.Id));
        }

        [Test]
        public async Task DeleteGenre_GenreExists_Returns_OkResult()
        {
            _genreService.Setup(s => s.Remove((It.IsAny<int>()))).ReturnsAsync(true);

            var result = await _genreController.DeleteGenre(It.IsAny<int>());

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteGenre_GenreDoesNotExist_Returns_NotFoundResult()
        {
            _genreService.Setup(s => s.Remove(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _genreController.DeleteGenre(It.IsAny<int>());

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
