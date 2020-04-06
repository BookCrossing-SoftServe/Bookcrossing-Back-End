using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTest.Controllers
{
    class BookControllerTest
    {
        [Test]
        public async Task TestGetAll()
        {
            var serviceMock = new Mock<IBook>();
            serviceMock.Setup(s => s.GetAll()).ReturnsAsync((GetTestBooks()));
            var booksController = new BooksController(serviceMock.Object);

            var getAllBooksResult = await booksController.GetAllBooks();

            var okResult = getAllBooksResult.Result as OkObjectResult;
            Assert.IsInstanceOf<OkObjectResult>(okResult);
            var books = okResult.Value as List<BookDto>;
            Assert.AreEqual(GetTestBooks().Count, books.Count());
        }

        private List<BookDto> GetTestBooks()
        {
            var books = new List<BookDto>
            {
                new BookDto()
                {
                    UserId = 1,
                    Name = "CLR via C#",
                    Available = true,
                    Publisher = "Kolosok"

                },
                new BookDto()
                {
                    UserId = 1,
                    Name = "CLR via C#",
                    Available = true,
                    Publisher = "Kolosok"

                }
            };
            return books;
        }
    }
}
