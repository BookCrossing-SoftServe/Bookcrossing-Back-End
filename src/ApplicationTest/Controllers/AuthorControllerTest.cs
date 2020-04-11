using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using BookCrossingBackEnd.Filters;
using BookCrossingBackEnd.Validators;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ApplicationTest.Controllers
{
    class AuthorControllerTest
    {
        private AuthorsController _authorController;
        private Mock<IAuthorService> _authorServiceMock;
        private AuthorDto _expectedAuthorDto;

        [OneTimeSetUp]
        public void Setup()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _authorController = new AuthorsController(_authorServiceMock.Object);
            _expectedAuthorDto = new AuthorDto() {Id = 201, FirstName = "Max", LastName = "Novitskyi", MiddleName = "Yar"};
        }


        //GET by ID
        [Test]
        public async Task GetAuthor_AuthorExists_Returns_OkObjectResult()
        {
            var requestedId = 201;
            _authorServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(_expectedAuthorDto);

            var result = await _authorController.GetAuthor(requestedId);
            var author = (AuthorDto)((OkObjectResult)result).Value;

            result.Should().BeOfType<OkObjectResult>();
            author.Id.Should().Be(requestedId);
        }

        [Test]
        public async Task GetAuthor_AuthorDoesNotExist_Returns_NotFoundResult()
        {
            _authorServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(null as AuthorDto);

            var result = await _authorController.GetAuthor(It.IsAny<int>());

            result.Should().BeOfType<NotFoundResult>();
        }


        //DELETE
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


        //POST
        [Test]
        public async Task Add_Author_Returns_CreatedAtAction()
        {
            var insertDto = new InsertAuthorDto() {FirstName = "Max", LastName = "Novitskyi", MiddleName = "Yar" };
            _authorServiceMock.Setup(s => s.Add(It.IsAny<InsertAuthorDto>())).ReturnsAsync(_expectedAuthorDto);

            var createdAtActionResult = await _authorController.PostAuthor(insertDto);
            var result = (AuthorDto) ((CreatedAtActionResult)createdAtActionResult.Result).Value;

            result.Should().BeOfType<AuthorDto>();
            insertDto.Should().BeEquivalentTo(result, options => options.Excluding(a => a.Id));
        }

        //PUT
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

        //Controller
        [Ignore("Authorization filter is disabled until fully implemented")]
        [Test]
        public void AuthorsController_has_Authorized_Attribute()
        {
            var controller = typeof(AuthorsController);

            controller.Should().BeDecoratedWith<AuthorizeAttribute>();
        }
    }
}
