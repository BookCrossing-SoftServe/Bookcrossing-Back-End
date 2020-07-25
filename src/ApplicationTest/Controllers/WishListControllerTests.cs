using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    internal class WishListControllerTests
    {
        private Mock<IWishListService> _wishListServiceMock;
        private WishListController _controller;

        [OneTimeSetUp]
        public void InitializeClass()
        {
            _wishListServiceMock = new Mock<IWishListService>();
            _controller = new WishListController(_wishListServiceMock.Object);
        }

        [SetUp]
        public void InitializeTest()
        {
            _wishListServiceMock.Invocations.Clear();
        }

        [Test]
        public async Task GetCurrentUserWishList_NoExceptionsWasThrown_ReturnsObjectResultWithPaginatedBookGetDto()
        {
            var pageableParams = new PageableParams();
            var paginatedBooks = new PaginationDto<BookGetDto>
            {
                Page = new List<BookGetDto>(),
                TotalCount = 0
            };
            _wishListServiceMock.Setup(obj => obj.GetWishesOfCurrentUserAsync(pageableParams))
                .ReturnsAsync(paginatedBooks);

            var result = await _controller.GetCurrentUserWishList(pageableParams);

            _wishListServiceMock.Verify(obj => obj.GetWishesOfCurrentUserAsync(pageableParams), Times.Once);
            result.Value.Should().Be(paginatedBooks);
        }

        [Test]
        public async Task AddWish_ServiceMethodThrowsInvalidOperationException_ReturnsForbiddenStatusCodeWishMessage()
        {
            var exceptionMessage = "User cannot add his book to wish list";
            _wishListServiceMock.Setup(obj => obj.AddWishAsync(It.IsAny<int>()))
                .Throws(new InvalidOperationException(exceptionMessage));

            var result = await _controller.AddWish(It.IsAny<int>());

            var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(403);
            objectResult.Value.Should().Be(exceptionMessage);
        }

        [Test]
        public async Task AddWish_NoExceptionsWasThrown_ShouldCallAddWishMethodFromService()
        {
            var bookId = 1;

            var result = await _controller.AddWish(bookId);

            _wishListServiceMock.Verify(obj => obj.AddWishAsync(bookId));

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteWish_NoExceptionsWasThrown_ShouldCallRemoveWishMethodFromService()
        {
            var bookId = 1;

            var result = await _controller.DeleteWish(bookId);

            _wishListServiceMock.Verify(obj => obj.RemoveWishAsync(bookId));

            result.Should().BeOfType<OkResult>();
        }
    }
}
