using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    internal class AuthorControllerTest
    {
        private AuthorsController _authorController;
        private Mock<IAuthorService> _authorServiceMock;

        [OneTimeSetUp]
        public void Setup()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _authorController = new AuthorsController(_authorServiceMock.Object);
        }


        #region GetById

        [Test]
        [TestCase(201)]
        public async Task GetAuthor_AuthorExists_Returns_OkObjectResultWithRequestedId(int id)
        {
            var expectedAuthor = new AuthorDto() {Id = 201};
            _authorServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(expectedAuthor);

            var result = await _authorController.GetAuthor(id);
            var author = (AuthorDto) ((OkObjectResult) result).Value;

            result.Should().BeOfType<OkObjectResult>();
            author.Id.Should().Be(id);
        }

        [Test]
        public async Task GetAuthor_AuthorDoesNotExist_Returns_NotFoundResult()
        {
            _authorServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(null as AuthorDto);

            var result = await _authorController.GetAuthor(It.IsAny<int>());

            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion GetById

        #region Delete

        [Test]
        public async Task DeleteAuthor_AuthorExists_Returns_OkResult()
        {
            _authorServiceMock.Setup(s => s.Remove((It.IsAny<int>()))).ReturnsAsync(true);

            var result = await _authorController.DeleteAuthor(It.IsAny<int>());

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteAuthor_AuthorDoesNotExist_Returns_NotFoundResult()
        {
            _authorServiceMock.Setup(s => s.Remove(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _authorController.DeleteAuthor(It.IsAny<int>());

            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion Delete

        #region Post

        [Test]
        public async Task Add_Author_Returns_ActionResultWithSamePropertyValues()
        {
            var insertDto = new AuthorDto() {FirstName = "Max", LastName = "Novitskyi"};
            var expectedAuthorDto = new AuthorDto()
                {Id = 201, FirstName = "Max", LastName = "Novitskyi"};
            _authorServiceMock.Setup(s => s.Add(It.IsAny<AuthorDto>())).ReturnsAsync(expectedAuthorDto);

            var createdAtActionResult = await _authorController.PostAuthor(insertDto);
            var result = (AuthorDto) ((CreatedAtActionResult) createdAtActionResult.Result).Value;

            result.Should().BeOfType<AuthorDto>();
            createdAtActionResult.Should().BeOfType<ActionResult<AuthorDto>>();
            insertDto.Should().BeEquivalentTo(result, options => options.Excluding(a => a.Id));
        }

        #endregion Post

        #region Put

        [Test]
        public async Task PutAuthor_AuthorExists_Returns_NoContent()
        {
            _authorServiceMock.Setup(s => s.Update(It.IsAny<AuthorDto>())).ReturnsAsync(true);

            var result = await _authorController.PutAuthor(It.IsAny<AuthorDto>());

            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutAuthor_AuthorDoesNotExist_Return_NotFound()
        {
            _authorServiceMock.Setup(s => s.Update(It.IsAny<AuthorDto>())).ReturnsAsync(false);

            var result = await _authorController.PutAuthor(It.IsAny<AuthorDto>());

            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion Put

        [Ignore("Authorization filter is disabled until fully implemented")]
        [Test]
        public void AuthorsController_has_Authorized_Attribute()
        {
            var controller = typeof(AuthorsController);

            controller.Should().BeDecoratedWith<AuthorizeAttribute>();
        }
    }
}