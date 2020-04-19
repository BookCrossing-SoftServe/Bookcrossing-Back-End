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

namespace ApplicationTest.Services
{
    [TestFixture]
    class BookServiceTest
    {
        private IBookService _bookService;
        private Mock<IRepository<Book>> _bookRepositoryMock;
        private Mock<IRepository<BookAuthor>> _bookAuthorRepositoryMock;
        private Mock<IRepository<BookGenre>> _bookGenreRepositoryMock;
        //private Mock<IMapper> _mapper;
        private BookCrossingContext _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _bookRepositoryMock = new Mock<IRepository<Book>>();
            _bookAuthorRepositoryMock = new Mock<IRepository<BookAuthor>>();
            _bookGenreRepositoryMock = new Mock<IRepository<BookGenre>>();
            //_mapper = new Mock<IMapper>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.Mapper());
            });
            var _mapper = mappingConfig.CreateMapper();
            var options = new DbContextOptionsBuilder<BookCrossingContext>().UseInMemoryDatabase(databaseName: "Fake DB").ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _context = new BookCrossingContext(options);
            _bookService = new BookService(_bookRepositoryMock.Object, _mapper, _bookAuthorRepositoryMock.Object, _bookGenreRepositoryMock.Object, _context);
        }

        [SetUp]
        public void SetUp()
        {
            _bookRepositoryMock.Reset();
        }


        [TestCase(1)]
        public async Task GetById_BookExists_Returns_BookDtoWithRequestedId(int id)
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            // _mapper.Setup(s => s.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto() { Id = id });

            var bookResult = await _bookService.GetById(id);

            bookResult.Should().BeOfType<BookDto>();
            bookResult.Id.Should().Be(id);
        }

        private List<Book> GetTestBooks()
        {
            return new List<Book>
            {
                new Book(){ Id = 1},
                new Book(){ Id = 2}
            };
        }

        [TestCase(3)]
        public async Task GetById_BookDoesNotExist_Returns_Null(int id)
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var bookResult = await _bookService.GetById(id);

            bookResult.Should().BeNull();
        }

        [Test]
        public async Task GetAll_Returns_ListOfBookkWithSameCount()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var booksResult = await _bookService.GetAll();

            booksResult.Should().HaveCount(booksMock.Object.Count()).And.BeAssignableTo<IEnumerable<BookDto>>();
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

        [TestCase(1)]
        public async Task Remove_BookExists_Returns_True(int id)
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            _bookRepositoryMock.Setup(s => s.Remove(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var deleteResult = await _bookService.Remove(id);

            deleteResult.Should().BeTrue();
            _bookRepositoryMock.Verify(x => x.Remove(It.IsAny<Book>()), Times.Once);
            _bookRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase(3)]
        public async Task Remove_BookDoesNotExist_Returns_False(int id)
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            _bookRepositoryMock.Setup(s => s.Remove(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var deleteResult = await _bookService.Remove(id);

            deleteResult.Should().BeFalse();
        }

        [TestCase(1)]
        public async Task Update_BookExists_Returns_True(int id)
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
            var bookDto = new BookDto() { Id = id };

            var result = await _bookService.Update(bookDto);

            result.Should().BeTrue();
        }

        [TestCase(3)]
        public async Task Update_BookDoesNotExist_Returns_False(int id)
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
            var bookDto = new BookDto() { Id = id };

            var result = await _bookService.Update(bookDto);

            result.Should().BeFalse();
        }
    }
}
