using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationTest.Controllers.Comment.Book
{
    [TestFixture]
    class ChildControllerTest
    {
        private BookChildCommentsController _bookChildCommentsController;
        private Mock<IBookChildCommentService> _mockChildBookCommentService;
        private Mock<IUserResolverService> _mockUserResolverService;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockChildBookCommentService = new Mock<IBookChildCommentService>();
            _mockUserResolverService = new Mock<IUserResolverService>();
            _bookChildCommentsController = new BookChildCommentsController(_mockChildBookCommentService.Object, _mockUserResolverService.Object);
        }


        #region Delete

        [Test]
        public async Task Delete_ByUser_Returns_OkObjectResult()
        {
            ChildDeleteDto deleteDto = new ChildDeleteDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
                OwnerId = 1
            };
            _mockChildBookCommentService.Setup(s => s.Remove(deleteDto.Ids)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(false);

            var deleteResult = await _bookChildCommentsController.Delete(deleteDto);

            var okResult = deleteResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            int deletedNumber = Convert.ToInt32(okResult.Value);
            deletedNumber.Should().Be(1);
        }

        [Test]
        public async Task Delete_ByUser_Returns_ForbidResult()
        {
            ChildDeleteDto deleteDto = new ChildDeleteDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
                OwnerId = 1
            };
            _mockChildBookCommentService.Setup(s => s.Remove(deleteDto.Ids)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(false);

            var deleteResult = await _bookChildCommentsController.Delete(deleteDto);

            var forbidResult = deleteResult.Result as ForbidResult;
            forbidResult.Should().BeOfType<ForbidResult>();
        }

        [Test]
        public async Task Delete_ByUser_Returns_NotFoundObjectResult()
        {
            ChildDeleteDto deleteDto = new ChildDeleteDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
                OwnerId = 1
            };
            _mockChildBookCommentService.Setup(s => s.Remove(deleteDto.Ids)).ReturnsAsync(0);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(false);

            var deleteResult = await _bookChildCommentsController.Delete(deleteDto);

            var notFoundObjectResult = deleteResult.Result as NotFoundObjectResult;
            notFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
            int deletedNumber = Convert.ToInt32(notFoundObjectResult.Value);
            deletedNumber.Should().Be(0);
        }

        [Test]
        public async Task Delete_ByAdmin_Returns_OkObjectResult()
        {
            ChildDeleteDto deleteDto = new ChildDeleteDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
                OwnerId = 1
            };
            _mockChildBookCommentService.Setup(s => s.Remove(deleteDto.Ids)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(true);

            var deleteResult = await _bookChildCommentsController.Delete(deleteDto);

            var okResult = deleteResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            int deletedNumber = Convert.ToInt32(okResult.Value);
            deletedNumber.Should().Be(1);
        }

        [Test]
        public async Task Delete_ByAdmin_Returns_NotFoundObjectResult()
        {
            ChildDeleteDto deleteDto = new ChildDeleteDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
                OwnerId = 1
            };
            _mockChildBookCommentService.Setup(s => s.Remove(deleteDto.Ids)).ReturnsAsync(0);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(true);

            var deleteResult = await _bookChildCommentsController.Delete(deleteDto);

            var notFoundObjectResult = deleteResult.Result as NotFoundObjectResult;
            notFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
            int deletedNumber = Convert.ToInt32(notFoundObjectResult.Value);
            deletedNumber.Should().Be(0);
        }

        #endregion Delete


        #region Post      

        [Test]
        public async Task Post_Returns_OkObjectResult()
        {
            ChildInsertDto insertDto = new ChildInsertDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0" },
                OwnerId = 1,
                Text = "Text"
            };
            _mockChildBookCommentService.Setup(s => s.Add(insertDto)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);

            var postResult = await _bookChildCommentsController.Post(insertDto);

            var okResult = postResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            int insertedNumber = Convert.ToInt32(okResult.Value);
            insertedNumber.Should().Be(1);
        }

        [Test]
        public async Task Post_Returns_BadRequestObjectResult()
        {
            ChildInsertDto insertDto = new ChildInsertDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0" },
                OwnerId = 1,
                Text = "Text"
            };
            _mockChildBookCommentService.Setup(s => s.Add(insertDto)).ReturnsAsync(0);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);

            var postResult = await _bookChildCommentsController.Post(insertDto);

            var badRequest = postResult.Result as BadRequestObjectResult;
            badRequest.Should().BeOfType<BadRequestObjectResult>();
            int insertedNumber = Convert.ToInt32(badRequest.Value);
            insertedNumber.Should().Be(0);
        }

        [Test]
        public async Task Post_Returns_ForbidResult()
        {
            ChildInsertDto insertDto = new ChildInsertDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0" },
                OwnerId = 1,
                Text = "Text"
            };
            _mockChildBookCommentService.Setup(s => s.Add(insertDto)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);

            var postResult = await _bookChildCommentsController.Post(insertDto);

            var forbidResult = postResult.Result as ForbidResult;
            forbidResult.Should().BeOfType<ForbidResult>();
        }

        #endregion Post


        #region Put

        [Test]
        public async Task Put_ByUser_Returns_OkObjectResult()
        {
            ChildUpdateDto updateDto = new ChildUpdateDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
                OwnerId = 1,
                Text = "Text"
            };
            _mockChildBookCommentService.Setup(s => s.Update(updateDto)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);

            var putResult = await _bookChildCommentsController.Put(updateDto);

            var okResult = putResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            int updatedNumber = Convert.ToInt32(okResult.Value);
            updatedNumber.Should().Be(1);
        }

        [Test]
        public async Task Put_ByUser_Returns_ForbidResult()
        {
            ChildUpdateDto updateDto = new ChildUpdateDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
                OwnerId = 1,
                Text = "Text"
            };
            _mockChildBookCommentService.Setup(s => s.Update(updateDto)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);

            var putResult = await _bookChildCommentsController.Put(updateDto);

            var forbidResult = putResult.Result as ForbidResult;
            forbidResult.Should().BeOfType<ForbidResult>();
        }

        [Test]
        public async Task Put_ByUser_Returns_NotFoundObjectResult()
        {
            ChildUpdateDto updateDto = new ChildUpdateDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
                OwnerId = 1,
                Text = "Text"
            };
            _mockChildBookCommentService.Setup(s => s.Update(updateDto)).ReturnsAsync(0);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);

            var putResult = await _bookChildCommentsController.Put(updateDto);

            var notFoundObjectResult = putResult.Result as NotFoundObjectResult;
            notFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
            int updatedNumber = Convert.ToInt32(notFoundObjectResult.Value);
            updatedNumber.Should().Be(0);
        }

        #endregion Put
    }
}
