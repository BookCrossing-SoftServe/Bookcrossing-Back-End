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
using Application.Dto.QueryParams;

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
            var testBooks = new List<BookGetDto>()
                {
                    new BookGetDto(),
                    new BookGetDto()
                };
            var testPagination = new Application.Dto.PaginationDto<BookGetDto>() { Page = testBooks };
            _bookService.Setup(s => s.GetAllAsync(It.IsAny<BookQueryParams>())).ReturnsAsync(testPagination);
            var query = new BookQueryParams() { Page = 1, PageSize = 2 };

            var getAllBooksResult = await _booksController.GetAllBooksAsync(query);

            var okResult = getAllBooksResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var books = okResult.Value as PaginationDto<BookGetDto>;
            books.Page.Should().HaveCount(testBooks.Count);
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
        public async Task PutBookAsync_BookDoesNotExist_Return_NotFound()
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
    }
}
