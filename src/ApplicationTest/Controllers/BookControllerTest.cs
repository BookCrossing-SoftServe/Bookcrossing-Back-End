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

        public BooksControllerTest()
        {
            _bookService = new Mock<IBookService>();
        }

        [Test]
        public async Task GetAllBooksTest()
        {
            var testBooks = GetTestBooks();
            _bookService.Setup(s => s.GetAll()).ReturnsAsync(testBooks);
            var booksController = new BooksController(_bookService.Object);

            var getAllBooksResult = await booksController.GetAllBooks();

            var okResult = getAllBooksResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var books = okResult.Value as List<BookDto>;
            books.Count().Should().Be(testBooks.Count);
        }

        private List<BookDto> GetTestBooks()
        {
            return new List<BookDto>
            {
                new BookDto(){Id = 1},
                new BookDto(){Id = 2}
            };
        }

        [Test]
        public async Task GetBookTest()
        {
            var testBook = GetTestBooks().FirstOrDefault();
            _bookService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(testBook);
            var booksController = new BooksController(_bookService.Object);

            var getBookResult = await booksController.GetBook(1);

            var okResult = getBookResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultBook = okResult.Value as BookDto;
            resultBook.Id.Should().Be(testBook.Id);
        }

        [Test]
        public async Task PutBookTest()
        {
            _bookService.Setup(m => m.Update(It.IsAny<BookDto>())).Verifiable();
            var booksController = new BooksController(_bookService.Object);
            var testBook = GetTestBooks().FirstOrDefault();

            var putBookResult = await booksController.PutBook(testBook.Id, testBook);

            putBookResult.Should().BeOfType<NoContentResult>();
            _bookService.Verify(m => m.Update(It.IsAny<BookDto>()), Times.Once);
        }

    }
}
