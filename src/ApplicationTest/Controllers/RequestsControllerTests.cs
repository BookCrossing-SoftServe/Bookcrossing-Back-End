using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    public class RequestsControllerTests
    {
        private RequestsController _requestController;
        private Mock<IRequestService> _requestServiceMock;
        private Mock<IUserResolverService> _userResolverServiceMock;

        [OneTimeSetUp]
        public void Setup()
        {
            _requestServiceMock = new Mock<IRequestService>();
            _userResolverServiceMock = new Mock<IUserResolverService>();
            _requestController = new RequestsController(_requestServiceMock.Object, _userResolverServiceMock.Object);
        }

        #region GetByBook

        [Test]
        public async Task GetRequestsForBook_BookExists_RequestQueryParamDoesNotExists_Returns_IEnumerable_OkResult()
        {
            var expectedRequests = new List<RequestDto>
            {
                new RequestDto()
                {
                    Id = 1,
                    RequestDate = new DateTime(1999, 05, 28),
                    User = new UserDto() { Id = 1 },
                    Book = new BookGetDto() { Id = 1 },
                    Owner = new UserDto() { Id = 1 },
                    ReceiveDate = null
                }
            };
            var queryParameter = new RequestsQueryParams();
            _requestServiceMock.Setup(s => s.GetAllByBookAsync(x => x.BookId == It.IsAny<int>()))
                .ReturnsAsync(expectedRequests);

            var result = await _requestController.GetByBook(It.IsAny<int>(), queryParameter);
            result.Result.Should().BeOfType<OkObjectResult>();
        }


        [Test]
        public async Task GetRequestsForBook_BookExists_RequestQueryParamExists_Returns_OkResult()
        {
            var expectedRequest = new RequestDto()
            {
                Id = 1,
                RequestDate = new DateTime(1999, 05, 28),
                User = new UserDto() {Id = 1},
                Book = new BookGetDto() {Id = 1},
                Owner = new UserDto() {Id = 1},
                ReceiveDate = null
            };

            var queryParameter = new RequestsQueryParams()
            {
                First = true,
                Last = false
            };
            _requestServiceMock.Setup(s => s.GetByBookAsync(It.IsAny<Expression<Func<Request,bool>>>(), queryParameter))
                .ReturnsAsync(expectedRequest);

            var result = await _requestController.GetByBook(It.IsAny<int>(), queryParameter);
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetRequestsForBook_BookDoesNotExists_RequestQueryParamExists_Returns_NotFoundResult()
        {
            var queryParameter = new RequestsQueryParams()
            {
                First = true,
                Last = false
            };
            _requestServiceMock.Setup(s => s.GetByBookAsync(It.IsAny<Expression<Func<Request, bool>>>(), queryParameter))
                .ReturnsAsync(null as RequestDto);

            var result = await _requestController.GetByBook(It.IsAny<int>(), queryParameter);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        #endregion GetByBook

        #region Remove

        [Test]
        public async Task RemoveRequest_RequestExists_Returns_OkResult()
        {
            _requestServiceMock.Setup(s => s.RemoveAsync((It.IsAny<int>()))).ReturnsAsync(true);

            var result = await _requestController.Remove(It.IsAny<int>());

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task RemoveRequest_RequestDoesNotExist_Returns_NotFoundResult()
        {
            _requestServiceMock.Setup(s => s.RemoveAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _requestController.Remove(It.IsAny<int>());

            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion Remove

        #region Make

        [Test]
        public async Task MakeRequest_BookIsNotAvailableForRequest_Returns_OkResult()
        {
            _userResolverServiceMock.Setup(x => x.GetUserId()).Returns(1);
            var userId = _userResolverServiceMock.Object.GetUserId();
            _requestServiceMock.Setup(s => s.MakeAsync(userId, It.IsAny<int>())).ReturnsAsync((RequestDto)null);
            var result = await _requestController.Make(It.IsAny<int>());
            result.Result.Should().BeOfType<NotFoundResult>();
        }
        [Test]
        public async Task MakeRequest_BookIsAvailableForRequest_Return_NoFound()
        {
            var expectedRequestDto = new RequestDto()
            {
                Id = 1,
                RequestDate = new DateTime(1999, 05, 28),
                User = new UserDto() { Id = 1 },
                Book = new BookGetDto() { Id = 1 },
                Owner = new UserDto() { Id = 1 },
                ReceiveDate = null
            };
            _userResolverServiceMock.Setup(x => x.GetUserId()).Returns(1);
            var userId = _userResolverServiceMock.Object.GetUserId();
            _requestServiceMock.Setup(s => s.MakeAsync(userId, It.IsAny<int>())).ReturnsAsync(expectedRequestDto);

            var result = await _requestController.Make(It.IsAny<int>());
            result.Should().BeOfType<ActionResult<RequestDto>>();
        }

        #endregion Make

        #region ApproveReceive

        [Test]
        public async Task ApproveReceive_RequestExists_Returns_OkResult()
        {
            _requestServiceMock.Setup(s => s.ApproveReceiveAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _requestController.ApproveReceive(It.IsAny<int>());

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task ApproveReceive_RequestDoesNotExist_Returns_NotFoundResult()
        {
            _requestServiceMock.Setup(s => s.ApproveReceiveAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _requestController.ApproveReceive(It.IsAny<int>());

            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion ApproveReceive

        [Test]
        public void RequestsController_hasAuthorizedAttribute_True()
        {
            var controller = typeof(RequestsController);

            controller.Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Test]
        public async Task GetByUser_RequestsWereFound_ReturnsPaginatedDtoListOfRequestDto()
        {
            var expectedRequests = new List<RequestDto>
            {
                new RequestDto()
                {
                    Id = 1,
                    RequestDate = new DateTime(1999, 05, 28),
                    User = new UserDto() { Id = 1 },
                    Book = new BookGetDto() { Id = 1 },
                    Owner = new UserDto() { Id = 1 },
                    ReceiveDate = null
                }
            };

            _requestServiceMock.Setup(m => m.GetAsync(
                    It.IsAny<Expression<Func<Request, bool>>>(),
                    It.IsAny<BookQueryParams>()))
                .ReturnsAsync(new PaginationDto<RequestDto>()
                {
                    Page = expectedRequests,
                    TotalCount = 1
                });

            var result = await _requestController.GetByUser(It.IsAny<BookQueryParams>());

            _userResolverServiceMock.Verify(m => m.GetUserId());
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<PaginationDto<RequestDto>>>();
            result.Value.Page.Should().NotBeNull().And.NotContainNulls();
        }

        [Test]
        public async Task GetByUser_RequestsWereNotFound_ReturnsPaginatedDtoListOfRequestDto()
        {
            _requestServiceMock.Setup(m => m.GetAsync(
                    It.IsAny<Expression<Func<Request, bool>>>(),
                    It.IsAny<BookQueryParams>()))
                .ReturnsAsync(value: null);

            var result = await _requestController.GetByUser(It.IsAny<BookQueryParams>());

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Value.Should().BeNull();
        }
    }
}
