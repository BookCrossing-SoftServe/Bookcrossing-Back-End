using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.OuterSource;
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
    internal class OuterBooksSourceControllerTests
    {
        private Mock<IOuterBookSourceService> _outerBookSourceServiceMock;
        private OuterBooksSourceController _controller;

        [OneTimeSetUp]
        public void InitializeClass()
        {
            _outerBookSourceServiceMock = new Mock<IOuterBookSourceService>();
            _controller = new OuterBooksSourceController(_outerBookSourceServiceMock.Object);
        }

        [SetUp]
        public void InitializeTest()
        {
            _outerBookSourceServiceMock.Invocations.Clear();
        }

        [Test]
        public async Task GetBooks_AnyQueryParameters_ReturnsObjectResultWithPaginationDtoWithoutNulls()
        {
            _outerBookSourceServiceMock.Setup(obj => obj.SearchBooks(It.IsAny<OuterSourceQueryParameters>()))
                .ReturnsAsync(new PaginationDto<OuterBookDto>
                {
                    Page = new List<OuterBookDto>(),
                    TotalCount = 0
                });

            var result = await _controller.GetBooks(It.IsAny<OuterSourceQueryParameters>());

            _outerBookSourceServiceMock.Verify(
                obj => obj.SearchBooks(It.IsAny<OuterSourceQueryParameters>()), 
                Times.Once);

            result.Should().NotBeNull();
            result.Value.Should().BeOfType<PaginationDto<OuterBookDto>>();
        }

        [Test]
        public async Task GetBook_GoodreadsServiceReturnsNull_ReturnsNotFountResult()
        {
            _outerBookSourceServiceMock.Setup(obj => obj.GetBook(It.Is<int>(x => x > 0)))
                .ReturnsAsync(value: null);

            var result = await _controller.GetBook(1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetBook_GoodreadsServiceReturnsOuterBookDtoObject_ReturnsObjectResultWithOuterBookDto()
        {
            _outerBookSourceServiceMock.Setup(obj => obj.GetBook(It.Is<int>(x => x > 0)))
                .ReturnsAsync(new OuterBookDto());

            var result = await _controller.GetBook(1);

            result.Should().NotBeNull();
            result.Value.Should().BeOfType<OuterBookDto>();
        }
    }
}
