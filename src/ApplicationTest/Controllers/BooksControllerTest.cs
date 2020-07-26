using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    class BooksControllerTest
    {
        private Mock<IBookService> _bookService;
        private BooksController _booksController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _bookService = new Mock<IBookService>();
            _booksController = new BooksController(_bookService.Object); ;
        }

        [Test]
        public async Task GetAllBooksAsync_BooksExist_ReturnsOkObjectResultWithRequestedCount()
        {
            _bookService.Setup(m => m.GetAllAsync(It.IsAny<BookQueryParams>()))
                .ReturnsAsync(new PaginationDto<BookGetDto>
                {
                    Page = new List<BookGetDto>(),
                    TotalCount = 1
                });

            var result = await _booksController.GetAllBooksAsync(It.IsAny<BookQueryParams>());

            result.Should().NotBeNull();
            result.Value.Should().BeOfType<PaginationDto<BookGetDto>>();
            result.Value.Page.Should().NotBeNull().And.NotContainNulls();
        }


        [Test]
        public async Task GetBookAsync_BookExists_Returns_OkObjectResultWithRequestedId()
        {
            var testBook = new BookGetDto() { Id = 1 };

            _bookService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testBook);

            var getBookResult = await _booksController.GetBook(It.IsAny<int>());

            var okResult = getBookResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultBook = okResult.Value as BookGetDto;
            resultBook.Id.Should().Be(testBook.Id);
        }

        private BookPutDto GetTestBook()
        {
            return new BookPutDto() { Id = 1 };
        }

        [Test]
        public async Task GetBookAsync_BookDoesNotExist_Returns_NotFoundResult()
        {
            _bookService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as BookGetDto);

            var result = await _booksController.GetBook(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutBookAsync_BookExists_Returns_NoContent()
        {
            var testBook = GetTestBook();
            _bookService.Setup(m => m.UpdateAsync(It.IsAny<BookPutDto>())).ReturnsAsync(true);

            var putBookResult = await _booksController.PutBookAsync(testBook.Id, testBook);

            putBookResult.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutBookAsync_IdNotEqualToBookDtoId_ReturnsBadRequest()
        {

            var testBook = GetTestBook();
            var id = 3;
            _bookService.Setup(m => m.UpdateAsync(It.IsAny<BookPutDto>())).ReturnsAsync(true);

            var putBookResult = await _booksController.PutBookAsync(id, testBook);

            putBookResult.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task PutBookAsync_BookDoesNotExist_Return_BadRequest()
        {
            var testBook = GetTestBook();
            _bookService.Setup(m => m.UpdateAsync(It.IsAny<BookPutDto>())).ReturnsAsync(false);

            var putBookResult = await _booksController.PutBookAsync(testBook.Id, testBook);

            putBookResult.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task DeleteBookAsync_BookExists_Returns_OkResult()
        {
            _bookService.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(true);

            var deleteBookResult = await _booksController.DeleteBookAsync(It.IsAny<int>());

            deleteBookResult.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteBookAsync_BookDoesNotExist_Returns_NotFoundResult()
        {
            _bookService.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(false);

            var deleteBookResult = await _booksController.DeleteBookAsync(It.IsAny<int>());

            deleteBookResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PostBookAsync_Success_ReturnsCreatedAtActionResult()
        {
            var testBook = new BookGetDto() { Id = 1};
            _bookService.Setup(m => m.AddAsync(It.IsAny<BookPostDto>())).ReturnsAsync(testBook);

            var createdAtActionResult = await _booksController.PostBookAsync(It.IsAny<BookPostDto>());
            var result = (BookGetDto)((CreatedAtActionResult)createdAtActionResult.Result).Value;

            result.Should().BeOfType<BookGetDto>();
            createdAtActionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            result.Should().BeEquivalentTo(testBook, options => options.Excluding(a => a.Id));
        }

        [Test]
        public async Task ActivateBookAsync_Fail_ReturnsBadRequest()
        {
            _bookService.Setup(m => m.ActivateAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _booksController.ActivateBookAsync(It.IsAny<int>());

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task ActivateBookAsync_Success_ReturnsNoContent()
        {
            _bookService.Setup(m => m.ActivateAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _booksController.ActivateBookAsync(It.IsAny<int>());

            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeactivateBookAsync_Fail_ReturnsBadRequest()
        {
            _bookService.Setup(m => m.DeactivateAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _booksController.DeactivateBookAsync(It.IsAny<int>());

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task DeactivateBookAsync_Success_ReturnsNoContent()
        {
            _bookService.Setup(m => m.DeactivateAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _booksController.DeactivateBookAsync(It.IsAny<int>());

            result.Should().BeOfType<NoContentResult>();
        }


        [Test]
        public async Task GetRegisteredBooksAsync_AnyBookQueryParams_ReturnsPaginatedDtoListOfBookGetDto()
        {
            _bookService.Setup(m => m.GetRegistered(It.IsAny<BookQueryParams>()))
                .ReturnsAsync(new PaginationDto<BookGetDto>
                {
                    Page = new List<BookGetDto>(),
                    TotalCount = 1
                });

            var result = await _booksController.GetRegisteredBooksAsync(It.IsAny<BookQueryParams>());

            result.Should().NotBeNull();
            result.Value.Should().BeOfType<PaginationDto<BookGetDto>>();
            result.Value.Page.Should().NotBeNull().And.NotContainNulls();
        }

        [Test]
        public async Task GetCurrentOwnedBooksAsync_AnyBookQueryParams_ReturnsPaginatedDtoListOfBookGetDto()
        {
            _bookService.Setup(m => m.GetCurrentOwned(It.IsAny<BookQueryParams>()))
                .ReturnsAsync(new PaginationDto<BookGetDto>
                {
                    Page = new List<BookGetDto>(),
                    TotalCount = 1
                });

            var result = await _booksController.GetCurrentOwnedBooksAsync(It.IsAny<BookQueryParams>());

            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<PaginationDto<BookGetDto>>>();
            result.Value.Page.Should().NotBeNull().And.NotContainNulls();
        }

        [Test]
        public async Task GetReadBooksAsync_AnyBookQueryParams_ReturnsPaginatedDtoListOfBookGetDto()
        {
            _bookService.Setup(m => m.GetReadBooksAsync(It.IsAny<BookQueryParams>()))
                .ReturnsAsync(new PaginationDto<BookGetDto>
                {
                    Page = new List<BookGetDto>(),
                    TotalCount = 1
                });

            var result = await _booksController.GetReadBooksAsync(It.IsAny<BookQueryParams>());

            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<PaginationDto<BookGetDto>>>();
            result.Value.Page.Should().NotBeNull().And.NotContainNulls();
        }
    }
}
