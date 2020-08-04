using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

namespace ApplicationTest.Services
{
    [TestFixture]
    public class UsersServiceTests
    {
        private BookCrossingContext _context;
        private UsersService _usersService;
        private Mock<IEmailSenderService> _emailSenderServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IRepository<User>> _userRepositoryMock;
        private Mock<IPaginationService> _paginationServiceMock;
        private Mock<IRepository<UserRoom>> _userRoomRepositoryMock;
        private Mock<IRepository<ResetPassword>> _resetPasswordRepositoryMock;
        private Mock<IBookService> _bookServiceMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _resetPasswordRepositoryMock = new Mock<IRepository<ResetPassword>>();
            _mapperMock = new Mock<IMapper>();
            _emailSenderServiceMock = new Mock<IEmailSenderService>();
            _userRoomRepositoryMock = new Mock<IRepository<UserRoom>>();
            _bookServiceMock = new Mock<IBookService>();
            _paginationServiceMock = new Mock<IPaginationService>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.MapperProfilers.AuthorProfile());
                mc.AddProfile(new Application.MapperProfilers.BookChildCommentProfile());
                mc.AddProfile(new Application.MapperProfilers.BookRootCommentProfile());
                mc.AddProfile(new Application.MapperProfilers.GenreProfile());
                mc.AddProfile(new Application.MapperProfilers.LocationProfile());
                mc.AddProfile(new Application.MapperProfilers.RequestProfile());
                mc.AddProfile(new Application.MapperProfilers.UserProfile());
                mc.AddProfile(new Application.MapperProfilers.BookProfile());
            });
            var _mapper = mappingConfig.CreateMapper();
            var options = new DbContextOptionsBuilder<BookCrossingContext>().UseInMemoryDatabase(databaseName: "Fake DB").ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _context = new BookCrossingContext(options);
            _usersService = new UsersService(_userRepositoryMock.Object, _mapper,_emailSenderServiceMock.Object,
                                                _resetPasswordRepositoryMock.Object, _userRoomRepositoryMock.Object, _bookServiceMock.Object,
                                                    _context, _paginationServiceMock.Object);
        }
        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock.Reset();
        }

        [Test]
        public async Task GetById_UserExists_Returns_UserDto()
        {
            var usersMock = GetTestUsers().AsQueryable().BuildMock();
            _userRepositoryMock.Setup(s => s.GetAll()).Returns(usersMock.Object);

            var usersResult = await _usersService.GetById(x=>x.Id == 1);

            usersResult.Should().BeOfType<UserDto>();
            usersResult.Id.Should().Be(1);
        }

        [Test]
        public async Task GetById_UserNotExists_ReturnsNull()
        {
            var usersMock = GetTestUsers().AsQueryable().BuildMock();
            _userRepositoryMock.Setup(s => s.GetAll()).Returns(usersMock.Object);

            var usersResult = await _usersService.GetById(x => x.Id == 3);

            usersResult.Should().BeNull();
        }

        private List<User> GetTestUsers()
        {
            return new List<User>
            {
                new User(){ Id = 1, Email = "test1@gmail.com", FirstName = "John", LastName = "Johnson", Password = "aaaa"},
                new User(){ Id = 2, Email = "test2@gmail.com", FirstName = "Anya", LastName = "Danya", Password = "1234"}
            };
        }

        [Test]
        public async Task AddUser_NewUser_ReturnsRegisterDto()
        {
            string expectedEmail = "123@gmail.com";
            _userRepositoryMock.Setup(s => s.FindByCondition(u => u.Email == expectedEmail)).ReturnsAsync(null as User);
            var user = new RegisterDto() { Id = 5, Email = expectedEmail, FirstName = "John", LastName = "Johnson", Password = "1234" };

            var usersResult = await _usersService.AddUser(user);

            usersResult.Email.Should().Be(expectedEmail);
        }

        [Test]
        [TestCase("test1@gmail.com")]
        public async Task AddUser_ExistingUser_ReturnsNull(string email)
        {
            var expectedUser = new User() { Id = 1, Email = "test1@gmail.com", FirstName = "John", LastName = "Johnson", Password = "aaaa" };
            _userRepositoryMock.Setup(s => s.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(expectedUser);


            var usersResult = await _usersService.AddUser(new RegisterDto() { Id = 3, Email = email, FirstName = "Vasyl", LastName = "Bohdanovych", Password = "__123A$90" });

            usersResult.Should().BeNull();
        }


        [Test]
        public async Task ForbidEmailNotifications_EmailExists_ReturnsTrue()
        {
            var usersMock = GetTestUsers().AsQueryable().BuildMock();
            var expectedUser = new User() { Id = 1, Email = "test1@gmail.com", FirstName = "John", LastName = "Johnson", Password = "aaaa" };
            _userRepositoryMock.Setup(s => s.GetAll()).Returns(usersMock.Object);
            _userRepositoryMock.Setup(s => s.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(expectedUser);
            string mail = "test1@gmail.com";

            var result = await _usersService.ForbidEmailNotification(new ForbidEmailDto() { Email = mail, Code = string.Join(null, SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(mail)).Select(x => x.ToString("x2"))) } );

            result.Should().BeTrue();
        }

        [Test]
        public async Task ForbidEmailNotifications_EmailNotExists_ReturnsFalse()
        {
            var usersMock = GetTestUsers().AsQueryable().BuildMock();
            _userRepositoryMock.Setup(s => s.GetAll()).Returns(usersMock.Object);
            string mail = "12345@gmail.com";

            var result = await _usersService.ForbidEmailNotification(new ForbidEmailDto() { Email = mail, Code = string.Join(null, SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(mail)).Select(x => x.ToString("x2"))) });

            result.Should().BeFalse();
        }


        [Test]
        public async Task UpdateUser_AffectedRows0_ThrowsDbUpdateException()
        {
            _userRepositoryMock.Setup(s => s.SaveChangesAsync()).ReturnsAsync(0);
            var updateUser = new UserUpdateDto()
            {
                Email = "notexisting@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                RoleId = 2,
                UserLocation = new RoomLocationDto() { Location = new LocationDto() { Id = 1, City = "Lviv" }, RoomNumber = "11" }
            };


            Func<Task> a = async () => await _usersService.UpdateUser(updateUser);


            a.Should().Throw<DbUpdateException>();
        }

        [Test]
        public async Task RemoveUser_AffectedRows0_ThrowsDbUpdateException()
        {
            int userId = 5;
            _userRepositoryMock.Setup(s => s.FindByIdAsync(userId)).ReturnsAsync(new User());
            _userRepositoryMock.Setup(s => s.SaveChangesAsync()).ReturnsAsync(0);

            Func<Task> a = async () => await _usersService.RemoveUser(userId);

            a.Should().Throw<DbUpdateException>();
        }

        [Test]
        public async Task RemoveUser_UserDoesNotExist_ThrowsObjectNotFound()
        {
            var userId = 5;
            _userRepositoryMock.Setup(s => s.FindByIdAsync(userId))
                .ReturnsAsync(value: null);

            Func<Task> a = async () => await _usersService.RemoveUser(userId);

            a.Should().Throw<ObjectNotFoundException>();
        }

        [Test]
        public async Task RemoveUser_UserExists_ShouldDeactivateUsersBooksAndRemoveHis()
        {
            var userId = 5;
            var books = new List<Book>
            {
                new Book {Id = 1},
                new Book {Id = 2}
            };
            var user = new User {Id = userId, Book = books };
            _userRepositoryMock.Setup(s => s.FindByIdAsync(userId))
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _usersService.RemoveUser(userId);

            foreach (var book in books)
            {
                _bookServiceMock.Verify(s => s.DeactivateAsync(book.Id), Times.Once);
            }

            _userRepositoryMock.Verify(r => r.Remove(user), Times.Once);
            _userRepositoryMock.Verify(r => r.SaveChangesAsync());
        }
    }
}
