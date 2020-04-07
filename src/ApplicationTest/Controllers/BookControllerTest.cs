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
    class BookControllerTest
    {
        private Mock<IBookService> _bookService;

        public BookControllerTest()
        {
            _bookService = new Mock<IBookService>();
        }

        [Test]
        public async Task TestGetAll()
        {
            _bookService.Setup(s => s.GetAll()).ReturnsAsync((GetTestBooks()));
            var booksController = new BooksController(_bookService.Object);

            var getAllBooksResult = await booksController.GetAllBooks();

            var okResult = getAllBooksResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var books = okResult.Value as List<BookDto>;
            books.Count().Should().Be(GetTestBooks().Count);
        }

        private List<BookDto> GetTestBooks()
        {
            return  new List<BookDto>
            {
                new BookDto(),
                new BookDto()
            };
        }

    }
}
