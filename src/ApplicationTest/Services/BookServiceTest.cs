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
using Domain.NoSQL;
using Domain.NoSQL.Entities;
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
        private Mock<IRepository<Language>> _bookLanguageRepositoryMock;
        private Mock<IRepository<User>> _userLocationServiceMock;
        private Mock<IUserResolverService> _userResolverServiceMock;
        private Mock<IRepository<Request>> _requestServiceMock;
        private Mock<IImageService> _imageServiceMock;
        private Mock<IEmailSenderService> _emailSenderServiceMock;
        private Mock<IRootRepository<BookRootComment>> _rootCommentRepository;
        private Mock<IHangfireJobScheduleService> _hangfireJobScheduleService;
        private Mock<IWishListService> _wishListServiceMock;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _bookRepositoryMock = new Mock<IRepository<Book>>();
            _bookAuthorRepositoryMock = new Mock<IRepository<BookAuthor>>();
            _bookGenreRepositoryMock = new Mock<IRepository<BookGenre>>();
            _bookLanguageRepositoryMock = new Mock<IRepository<Language>>();
            _requestServiceMock = new Mock<IRepository<Request>>();
            _userLocationServiceMock = new Mock<IRepository<User>>();
            _userResolverServiceMock = new Mock<IUserResolverService>();
            _imageServiceMock = new Mock<IImageService>();
            _emailSenderServiceMock = new Mock<IEmailSenderService>();
            _hangfireJobScheduleService = new Mock<IHangfireJobScheduleService>();
            _imageServiceMock = new Mock<IImageService>();
            _rootCommentRepository = new Mock<IRootRepository<BookRootComment>>();
            _wishListServiceMock = new Mock<IWishListService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.MapperProfilers.AuthorProfile());
                mc.AddProfile(new Application.MapperProfilers.BookChildCommentProfile());
                mc.AddProfile(new Application.MapperProfilers.BookRootCommentProfile());
                mc.AddProfile(new Application.MapperProfilers.GenreProfile());
                mc.AddProfile(new Application.MapperProfilers.LanguageProfile());
                mc.AddProfile(new Application.MapperProfilers.LocationProfile());
                mc.AddProfile(new Application.MapperProfilers.RequestProfile());
                mc.AddProfile(new Application.MapperProfilers.UserProfile());
                mc.AddProfile(new Application.MapperProfilers.BookProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            var pagination = new PaginationService(_mapper);
            _bookService = new BookService(
                _bookRepositoryMock.Object, 
                _mapper, 
                _bookAuthorRepositoryMock.Object, 
                _bookGenreRepositoryMock.Object,
                _bookLanguageRepositoryMock.Object, 
                _userLocationServiceMock.Object,  
                pagination,
                _requestServiceMock.Object,
                _userResolverServiceMock.Object,
                _imageServiceMock.Object,
                _hangfireJobScheduleService.Object,
                _emailSenderServiceMock.Object,
                _rootCommentRepository.Object,
                _wishListServiceMock.Object);

            var authorMock = GetBookAuthor().AsQueryable();
            var genreMock = GetBookGenre().AsQueryable();
            var languageMock = GetBookLanguage().AsQueryable();
            var usersMock = GetUsers().AsQueryable();
            _bookAuthorRepositoryMock.Setup(s => s.GetAll()).Returns(authorMock);
            _bookGenreRepositoryMock.Setup(s => s.GetAll()).Returns(genreMock);
            _bookLanguageRepositoryMock.Setup(s => s.GetAll()).Returns(languageMock);
            _userLocationServiceMock.Setup(s => s.GetAll()).Returns(usersMock);
        }

        [SetUp]
        public void SetUp()
        {
            _bookRepositoryMock.Reset();
        }


        [Test]
        public async Task GetBookByIdAsync_BookExists_ReturnsBookDtoWithRequestedId()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var bookResult = await _bookService.GetByIdAsync(1);

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
        public async Task GetBookByIdAsync_BookDoesNotExist_Returns_Null()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var bookResult = await _bookService.GetByIdAsync(3);

            bookResult.Should().BeNull();
        }
        [Test]
        public async Task AddBookAsync_BookIsValid_Returns_BookDto()
        {
            var bookDto = new BookPostDto();
            _bookRepositoryMock.Setup(s => s.Add(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _imageServiceMock.Setup(x => x.UploadImage(It.IsAny<IFormFile>())).ReturnsAsync("name.png");

            var bookResult = await _bookService.AddAsync(bookDto);

            bookResult.Should().BeOfType<BookGetDto>();
            _bookRepositoryMock.Verify(x => x.Add(It.IsAny<Book>()), Times.Once);
            _bookRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveBookAsync_BookExists_Returns_True()
        {
            var book = new Book();
            _bookRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            _bookRepositoryMock.Setup(s => s.Remove(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _imageServiceMock.Setup(x => x.DeleteImage(It.IsAny<string>()));

            var deleteResult = await _bookService.RemoveAsync(1);

            deleteResult.Should().BeTrue();
            _bookRepositoryMock.Verify(x => x.Remove(It.IsAny<Book>()), Times.Once);
            _bookRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveBookAsync_BookDoesNotExist_Returns_False()
        {
            var book = new Book();
            _bookRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Book);
            _bookRepositoryMock.Setup(s => s.Remove(It.IsAny<Book>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _imageServiceMock.Setup(x => x.DeleteImage(It.IsAny<string>()));

            var deleteResult = await _bookService.RemoveAsync(3);

            deleteResult.Should().BeFalse();
        }

        [Test]
        public async Task UpdateBook_BookExists_Returns_True()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            _bookRepositoryMock.Setup(s => s.Update(It.IsAny<Book>(), It.IsAny<List<string>>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var bookDto = new BookPutDto() { Id = 1, FieldMasks = new List<string>{"Name"}  };

            var result = await _bookService.UpdateAsync(bookDto);

            result.Should().BeTrue();
        }

        [Test]
        public async Task UpdateBook_BookDoesNotExist_Returns_False()
        {
            var booksMock = GetTestBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            _bookRepositoryMock.Setup(s => s.Update(It.IsAny<Book>(), It.IsAny<List<string>>()));
            _bookRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var bookDto = new BookPutDto() { Id = 3, FieldMasks = new List<string> { "Name" } };

            var result = await _bookService.UpdateAsync(bookDto);

            result.Should().BeFalse();
        }

        #region Filtering_GetAll
        private List<Book> GetPopulatedBooks()
        {
            var genres = GetBookGenre();
            var languages = GetBookLanguage();
            var authors = GetBookAuthor();
            var users = GetUsers();
            var list = new List<Book>
            {
                new Book(){ Id = 1, BookGenre = new List<BookGenre>() {genres[0],genres[1]}, BookAuthor = new List<BookAuthor>() {authors[0]}, Language = languages[0], LanguageId = 1, Name = "CLR", State = BookState.Available, User = users[0], UserId = 1},
                new Book(){ Id = 2, BookGenre = new List<BookGenre>() {genres[2]}, BookAuthor = new List<BookAuthor>() {authors[1]},Name = "Test", Language = languages[1], LanguageId = 2, State = BookState.Available, User = users[1], UserId = 2},
                new Book(){ Id = 3, BookGenre = new List<BookGenre>() {genres[3]},  BookAuthor = new List<BookAuthor>() {authors[2]},Name = "ICE CLR", Language = languages[2], LanguageId = 3, State = BookState.Available, User = users[0], UserId = 1},
                new Book(){ Id = 4, BookGenre = new List<BookGenre>() {genres[4]}, BookAuthor = new List<BookAuthor>() {authors[3],authors[4]},Name = "FIRE", Language = languages[0], LanguageId = 1, State = BookState.Available, User = users[0], UserId = 1},
            };
            return list;
        }

        private List<Language> GetBookLanguage()
        {
            return new List<Language>() {
                new Language() { Id = 1, Name = "English"},
                new Language() { Id = 2, Name = "Ukrainian"},
                new Language() { Id = 3, Name = "Russian"}
            };
        }
        private List<BookGenre> GetBookGenre()
        {
            return new List<BookGenre>() {
                new BookGenre() { Book = new Book() { Id = 1 }, Genre = new Genre() { Id = 1 }, BookId = 1, GenreId = 1},
                new BookGenre() { Book = new Book() { Id = 1 }, Genre = new Genre() { Id = 2 }, BookId = 1, GenreId = 2 },
                new BookGenre() { Book = new Book() { Id = 2 }, Genre = new Genre() { Id = 1 }, BookId = 2, GenreId = 1 },
                new BookGenre() { Book = new Book() { Id = 3 }, Genre = new Genre() { Id = 3 }, BookId = 3, GenreId = 3 },
                new BookGenre() { Book = new Book() { Id = 4 }, Genre = new Genre() { Id = 3 }, BookId = 4, GenreId = 3 }
            };
        }
        private List<BookAuthor> GetBookAuthor()
        {
            var authorMartin = new Author() { FirstName = "George", LastName = "Martin", Id = 1 };
            var authorJoaRowling = new Author() { FirstName = "Joanne", LastName = "Rowling", Id = 2 };
            var authorJonRowling = new Author() { FirstName = "John", LastName = "Rowling", Id = 3 };
            return new List<BookAuthor>() {
                new BookAuthor() {Book = new Book() {Id = 1}, Author = authorMartin, AuthorId = 1, BookId = 1},
                new BookAuthor() {Book = new Book() {Id = 2}, Author = authorJoaRowling, AuthorId = 2, BookId = 2},
                new BookAuthor() {Book = new Book() {Id = 3}, Author = authorJonRowling, AuthorId = 3, BookId = 3},
                new BookAuthor() {Book = new Book() {Id = 4}, Author = authorJonRowling, AuthorId = 3, BookId = 4},
                new BookAuthor() {Book = new Book() {Id = 4}, Author = authorJonRowling, AuthorId = 3, BookId = 4}
            };
        }
        private List<User> GetUsers()
        {
            return new List<User>()
            {
                new User() { Id = 1, UserRoom = new UserRoom() { Location = new Location() { Id = 1 }, RoomNumber = "1", LocationId = 1 }},
                new User() { Id = 2, UserRoom = new UserRoom() { Location = new Location() { Id = 2 }, RoomNumber = "2", LocationId = 2 }}
            };
        }
        [Test]
        public async Task GetAllBooks_WhenHasSearchTermWithOneWord_Returns_books_filtered_by_LastName()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            var query = new BookQueryParams() { Page = 1, PageSize = 10, SearchTerm = "Martin" };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(1);
        }
        [Test]
        public async Task GetAllBooks_WhenHasSearchTerm_Returns_books_filtered_by_Book_Title()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Languages = new int[] { 1, 2, 3 }, SearchTerm = "CLR" };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(2);
        }

        [Test]
        public async Task GetAllBooks_WhenHasSearchTermWitTwoWords_Returns_books_filtered_By_FirstName_And_LastName()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, SearchTerm = "John Rowling" };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(2);
        }
        [Test]
        public async Task GetAllBooks_WhenHasManyGenreIds_Returns_books_with_either_ids()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Genres = new[] { 1, 3 } };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(4);
        }
        [Test]
        public async Task GetAllBooks_WhenHasOneGenreId_Returns_books_containing_the_id()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Genres = new[] { 2 } };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(1);
        }
        [Test]
        public async Task GetAllBooks_WhenHasShowAvailableTrue_Returns_available_books()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, ShowAvailable = true };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(4);
        }
        [Test]
        public async Task GetAllBooks_WhenHasShowAvailableFalse_Returns_all_books()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, ShowAvailable = false };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(GetPopulatedBooks().Count());
        }
        [Test]
        public async Task GetAllBooks_WhenHasLocation_Returns_books_with_LocationId()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Location = 1 };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(3);
        }
        [Test]
        public async Task GetAllBooks_WheAllQueryParamsPresent_Returns_filtered_books()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();

            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);

            var query = new BookQueryParams() { Page = 1, PageSize = 10, Location = 1, Genres = new[] { 1 }, SearchTerm = "Martin", ShowAvailable = true };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Page.Should().HaveCount(1);
        }
        [Test]
        public async Task GetAllBooks_Returns_ListOfBookWithSameCount()
        {
            var booksMock = GetPopulatedBooks().AsQueryable().BuildMock();
            _bookRepositoryMock.Setup(s => s.GetAll()).Returns(booksMock.Object);
            var query = new BookQueryParams() { Page = 1, PageSize = 2 };

            var booksResult = await _bookService.GetAllAsync(query);

            booksResult.Should().BeOfType<PaginationDto<BookGetDto>>();
            booksResult.Page.Should().HaveCount(2);
        }
        #endregion Filtering_GetAll
    }
}
