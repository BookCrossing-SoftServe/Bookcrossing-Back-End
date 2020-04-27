using Application.Dto;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Infrastructure.RDBMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto.QueryParams;

namespace ApplicationTest.Services
{
    [TestFixture]
    class BookServiceTest
    {
        private IBookService _bookService;
        private Mock<IRepository<Book>> _bookRepositoryMock;
        private Mock<IRepository<BookAuthor>> _bookAuthorRepositoryMock;
        private Mock<IRepository<BookGenre>> _bookGenreRepositoryMock;
        private Mock<IRepository<UserLocation>> _userLocationServiceMock;
        private Mock<IPaginationService> _paginationServiceMock;
        private BookCrossingContext _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _bookRepositoryMock = new Mock<IRepository<Book>>();
            _bookAuthorRepositoryMock = new Mock<IRepository<BookAuthor>>();
            _bookGenreRepositoryMock = new Mock<IRepository<BookGenre>>();
            _paginationServiceMock = new Mock<IPaginationService>();
            _userLocationServiceMock = new Mock<IRepository<UserLocation>>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.Mapper());
            });
            var _mapper = mappingConfig.CreateMapper();
            var options = new DbContextOptionsBuilder<BookCrossingContext>().UseInMemoryDatabase(databaseName: "Fake DB").ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _context = new BookCrossingContext(options);
            _bookService = new BookService(_bookRepositoryMock.Object, _mapper, _bookAuthorRepositoryMock.Object, _bookGenreRepositoryMock.Object, _userLocationServiceMock.Object, _paginationServiceMock.Object, _context);
        }

        [SetUp]
        public void SetUp()
        {
            _bookRepositoryMock.Reset();
        }


        [Test]
        public async Task GetById_BookExists_Returns_BookDtoWithRequestedId()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var bookResult = await _bookService.GetById(1);

            bookResult.Should().BeOfType<BookDto>();
            bookResult.Id.Should().Be(1);
        }

        private List<Book> GetTestBooks()
        {
            return new List<Book>
            {
                new Book(){ Id = 1},
                new Book(){ Id = 2}
            };
        }

        [Test]
        public async Task GetById_BookDoesNotExist_Returns_Null()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var bookResult = await _bookService.GetById(3);

            bookResult.Should().BeNull();
        }
        [Ignore("TODO FIX")]
        [Test]
        public async Task GetAll_Returns_ListOfBookWithSameCount()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            var query = new BookQueryParams() {Page = 1, PageSize = 2};
            var testPagination = new Application.Dto.PaginationDto<BookDto>()
            {
                Page = new List<BookDto>
                    {
                        new BookDto(),
                        new BookDto()
                    }
            };

            _paginationServiceMock.Setup(s => s.GetPageAsync<BookDto, Book>(It.IsAny<IQueryable<Book>>(), It.IsAny<PageableParams>())).ReturnsAsync(testPagination);

            var booksResult = await _bookService.GetAll(query);

            booksResult.Should().BeOfType<PaginationDto<BookDto>>();
            booksResult.Page.Should().HaveCount(2);
        }

        [Test]
        public async Task Add_BookIsValid_Returns_BookDto()
        {
            var bookDto = new BookDto();
            _bookRepositoryMock.Setup(s => s.Add(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var bookResult = await _bookService.Add(bookDto);

            bookResult.Should().BeOfType<BookDto>();
            _bookRepositoryMock.Verify(x => x.Add(It.IsAny<Book>()), Times.Once);
            _bookRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Remove_BookExists_Returns_True()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            _bookRepositoryMock.Setup(s => s.Remove(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var deleteResult = await _bookService.Remove(1);

            deleteResult.Should().BeTrue();
            _bookRepositoryMock.Verify(x => x.Remove(It.IsAny<Book>()), Times.Once);
            _bookRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Remove_BookDoesNotExist_Returns_False()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            _bookRepositoryMock.Setup(s => s.Remove(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var deleteResult = await _bookService.Remove(3);

            deleteResult.Should().BeFalse();
        }

        [Test]
        public async Task Update_BookExists_Returns_True()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            _bookRepositoryMock.Setup(s => s.Update(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var bookAuthorsMock = new List<BookAuthor>().AsQueryable().BuildMock();
            var bookGenresMock = new List<BookGenre>().AsQueryable().BuildMock();
            _bookAuthorRepositoryMock.Setup(x => x.GetAll()).Returns(bookAuthorsMock.Object);
            _bookGenreRepositoryMock.Setup(x => x.GetAll()).Returns(bookGenresMock.Object);
            _bookAuthorRepositoryMock.Setup(s => s.RemoveRange(It.IsAny<IEnumerable<BookAuthor>>()));
            _bookGenreRepositoryMock.Setup(s => s.RemoveRange(It.IsAny<IEnumerable<BookGenre>>()));
            _bookAuthorRepositoryMock.Setup(s => s.AddRange(It.IsAny<IEnumerable<BookAuthor>>()));
            _bookGenreRepositoryMock.Setup(s => s.AddRange(It.IsAny<IEnumerable<BookGenre>>()));
            var bookDto = new BookDto() { Id = 1 };

            var result = await _bookService.Update(bookDto);

            result.Should().BeTrue();
        }

        [Test]
        public async Task Update_BookDoesNotExist_Returns_False()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            _bookRepositoryMock.Setup(s => s.Update(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var bookAuthorsMock = new List<BookAuthor>().AsQueryable().BuildMock();
            var bookGenresMock = new List<BookGenre>().AsQueryable().BuildMock();
            _bookAuthorRepositoryMock.Setup(x => x.GetAll()).Returns(bookAuthorsMock.Object);
            _bookGenreRepositoryMock.Setup(x => x.GetAll()).Returns(bookGenresMock.Object);
            _bookAuthorRepositoryMock.Setup(s => s.RemoveRange(It.IsAny<IEnumerable<BookAuthor>>()));
            _bookGenreRepositoryMock.Setup(s => s.RemoveRange(It.IsAny<IEnumerable<BookGenre>>()));
            _bookAuthorRepositoryMock.Setup(s => s.AddRange(It.IsAny<IEnumerable<BookAuthor>>()));
            _bookGenreRepositoryMock.Setup(s => s.AddRange(It.IsAny<IEnumerable<BookGenre>>()));
            var bookDto = new BookDto() { Id = 3 };

            var result = await _bookService.Update(bookDto);

            result.Should().BeFalse();
        }
    }
}
