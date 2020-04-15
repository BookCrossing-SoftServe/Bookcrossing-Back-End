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
        public async Task GetAllBooksAsync_Returns_OkObjectResultWithRequestedCount()
        {
            var testBooks = GetTestBooks();
            _bookService.Setup(s => s.GetAll()).ReturnsAsync(testBooks);

            var getAllBooksResult = await _booksController.GetAllBooksAsync();

            var okResult = getAllBooksResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var books = okResult.Value as List<BookDto>;
            books.Count().Should().Be(testBooks.Count);
        }

        private List<BookDto> GetTestBooks()
        {
            return new List<BookDto>
            {
                new BookDto(),
                new BookDto()
            };
        }

        [Test]
        public async Task GetBookAsync_BookExists_Returns_OkObjectResultWithRequestedId()
        {
            var testBook = GetTestBook();
            _bookService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(testBook);

            var getBookResult = await _booksController.GetBookAsync(It.IsAny<int>());

            var okResult = getBookResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultBook = okResult.Value as BookDto;
            resultBook.Id.Should().Be(testBook.Id);
        }

        private BookDto GetTestBook()
        {
            return new BookDto() { Id = 1 };
        }

        [Test]
        public async Task GetBookAsync_BookDoesNotExist_Returns_NotFoundResult()
        {
            _bookService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(null as BookDto);

            var result = await _booksController.GetBookAsync(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutBookAsync_BookExists_Returns_NoContent()
        {
            var testBook = GetTestBook();
            _bookService.Setup(m => m.Update(It.IsAny<BookDto>())).ReturnsAsync(true);

            var putBookResult = await _booksController.PutBookAsync(testBook.Id, testBook);

            putBookResult.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutBookAsync_BookDoesNotExist_Return_NotFound()
        {
            var testBook = GetTestBook();
            _bookService.Setup(m => m.Update(It.IsAny<BookDto>())).ReturnsAsync(false);

            var putBookResult = await _booksController.PutBookAsync(testBook.Id, testBook);

            putBookResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteBookAsync_BookExists_Returns_OkResult()
        {
            _bookService.Setup(m => m.Remove(It.IsAny<int>())).ReturnsAsync(true);

            var deleteBookResult = await _booksController.DeleteBookAsync(It.IsAny<int>());

            deleteBookResult.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteBookAsync_BookDoesNotExist_Returns_NotFoundResult()
        {
            _bookService.Setup(m => m.Remove(It.IsAny<int>())).ReturnsAsync(false);

            var deleteBookResult = await _booksController.DeleteBookAsync(It.IsAny<int>());

            deleteBookResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PostBookAsync_Returns_CreatedAtActionResult()
        {
            var testBook = GetTestBook();
            _bookService.Setup(m => m.Add(It.IsAny<BookDto>())).ReturnsAsync(testBook);

            var createdAtActionResult = await _booksController.PostBookAsync(It.IsAny<BookDto>());
            var result = (BookDto)((CreatedAtActionResult)createdAtActionResult.Result).Value;

            result.Should().BeOfType<BookDto>();
            createdAtActionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            result.Should().BeEquivalentTo(testBook, options => options.Excluding(a => a.Id));
        }
    }
}
