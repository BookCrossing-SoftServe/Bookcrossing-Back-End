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
using Microsoft.AspNetCore.Http;

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
        private Mock<IUserResolverService> _userResolverServiceMock;
        private Mock<IRepository<Request>> _requestServiceMock;
        private Mock<IImageService> _imageServiceMock;
        private IMapper _mapper;
        private BookCrossingContext _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _bookRepositoryMock = new Mock<IRepository<Book>>();
            _bookAuthorRepositoryMock = new Mock<IRepository<BookAuthor>>();
            _bookGenreRepositoryMock = new Mock<IRepository<BookGenre>>();
            _paginationServiceMock = new Mock<IPaginationService>();
            _requestServiceMock = new Mock<IRepository<Request>>();
            _userLocationServiceMock = new Mock<IRepository<UserLocation>>();
            _userResolverServiceMock = new Mock<IUserResolverService>();
            _imageServiceMock = new Mock<IImageService>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.Mapper());
            });
            _mapper = mappingConfig.CreateMapper();
            var pagination = new PaginationService(_mapper);
            var options = new DbContextOptionsBuilder<BookCrossingContext>().UseInMemoryDatabase(databaseName: "Fake DB").ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _context = new BookCrossingContext(options);
            _bookService = new BookService(_bookRepositoryMock.Object, _mapper, _bookAuthorRepositoryMock.Object, _bookGenreRepositoryMock.Object,
                _userLocationServiceMock.Object, pagination,_requestServiceMock.Object, _context, _userResolverServiceMock.Object, _imageServiceMock.Object);
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

            bookResult.Should().BeOfType<BookGetDto>();
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
        [Test]
        public async Task Add_BookIsValid_Returns_BookDto()
        {
            var bookDto = new BookPostDto();
            _bookRepositoryMock.Setup(s => s.Add(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _imageServiceMock.Setup(x => x.UploadImage(It.IsAny<IFormFile>())).ReturnsAsync("name.png");

            var bookResult = await _bookService.Add(bookDto);

            bookResult.Should().BeOfType<BookGetDto>();
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
            _imageServiceMock.Setup(x => x.DeleteImage(It.IsAny<string>()));

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
            _imageServiceMock.Setup(x => x.DeleteImage(It.IsAny<string>()));

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
            var bookDto = new BookPutDto() { Id = 1 };

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
            var bookDto = new BookPutDto() { Id = 3 };

            var result = await _bookService.Update(bookDto);

            result.Should().BeFalse();
        }

        #region Filtering_GetAll
        private List<Book> GetPopulatedBooks()
        {
            var genre1 = new BookGenre() { Book = new Book() { Id = 1 }, Genre = new Genre() { Id = 1 } };
            var genre2 = new BookGenre() { Book = new Book() { Id = 1 }, Genre = new Genre() { Id = 2 } };
            var genre3 = new BookGenre() { Book = new Book() { Id = 2 }, Genre = new Genre() { Id = 1 } };
            var genre5 = new BookGenre() { Book = new Book() { Id = 3 }, Genre = new Genre() { Id = 3 } };

            var authorMartin = new Author() { FirstName = "George", LastName = "Martin", Id = 1 };
            var authorJoaRowling = new Author() { FirstName = "Joanne", LastName = "Rowling", Id = 2 };
            var authorJonRowling = new Author() { FirstName = "John", LastName = "Rowling", Id = 3 };
            var author1 = new BookAuthor() { Book = new Book() { Id = 1 }, Author = authorMartin, AuthorId = 1, BookId = 1 };
            var author2 = new BookAuthor() { Book = new Book() { Id = 1 }, Author = authorJoaRowling, AuthorId = 2, BookId = 2 };
            var author3 = new BookAuthor() { Book = new Book() { Id = 1 }, Author = authorJonRowling, AuthorId = 3, BookId = 3 };

            var user1 = new User() { UserLocation = new List<UserLocation>() { new UserLocation() { Location = new Location() { Id = 1 } } } };
            var user2 = new User() { UserLocation = new List<UserLocation>() { new UserLocation() { Location = new Location() { Id = 2 } } } };
            var list = new List<Book>
            {
                new Book(){ Id = 1, BookGenre = new List<BookGenre>() {genre1,genre2}, BookAuthor = new List<BookAuthor>() {author1}, Name = "CLR", Available = true, User = user1},
                new Book(){ Id = 2, BookGenre = new List<BookGenre>() {genre3}, BookAuthor = new List<BookAuthor>() {author2},Name = "Test", Available = true, User = user2},
                new Book(){ Id = 3, BookGenre = new List<BookGenre>() {genre5},  BookAuthor = new List<BookAuthor>() {author3},Name = "ICE CLR", Available = false, User = user1},
                new Book(){ Id = 4, BookGenre = new List<BookGenre>() {}, BookAuthor = new List<BookAuthor>() {author1,author2},Name = "FIRE", Available = false, User = user1},
            };
            return list;
        }

        [Test]
        public async Task GetAll_WhenHasSearchTermWithOneWord_Returns_books_filtered_by_LastName()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, SearchTerm = "Martin" };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(2);
        }
        [Test]
        public async Task GetAll_WhenHasSearchTerm_Returns_books_filtered_by_Book_Title()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, SearchTerm = "CLR" };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(2);
        }

        [Test]
        public async Task GetAll_WhenHasSearchTermWitTwoWords_Returns_books_filtered_By_FirstName_And_LastName()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, SearchTerm = "John Rowling" };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(1);
        }
        [Test]
        public async Task GetAll_WhenHasManyGenreIds_Returns_books_with_either_ids()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Genres = new[] { 1, 3 } };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(3);
        }
        [Test]
        public async Task GetAll_WhenHasOneGenreId_Returns_books_containing_the_id()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Genres = new[] { 2 } };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(1);
        }
        [Test]
        public async Task GetAll_WhenHasShowAvailableTrue_Returns_available_books()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, ShowAvailable = true };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(2);
        }
        [Test]
        public async Task GetAll_WhenHasShowAvailableFalse_Returns_all_books()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, ShowAvailable = false };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(GetPopulatedBooks().Count());
        }
        [Test]
        public async Task GetAll_WhenHasLocation_Returns_books_with_LocationId()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Location = 1 };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(3);
        }
        [Test]
        public async Task GetAll_WheAllQueryParamsPresent_Returns_filtered_books()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Location = 1, Genres = new[] { 1 }, SearchTerm = "Martin", ShowAvailable = true };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Page.Should().HaveCount(1);
        }
        [Test]
        public async Task GetAll_Returns_ListOfBookWithSameCount()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            var query = new BookQueryParams() { Page = 1, PageSize = 2 };

            var booksResult = await _bookService.GetAll(query);

            booksResult.Should().BeOfType<PaginationDto<BookGetDto>>();
            booksResult.Page.Should().HaveCount(2);
        }
        #endregion Filtering_GetAll
    }
}
