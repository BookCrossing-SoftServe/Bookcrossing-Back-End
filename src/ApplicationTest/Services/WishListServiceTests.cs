using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Services
{
    [TestFixture]
    internal class WishListServiceTests
    {
        private Mock<IRepository<Wish>> _wishRepositoryMock;
        private Mock<IRepository<Book>> _bookRepositoryMock;
        private Mock<IUserResolverService> _userResolverServiceMock;
        private Mock<IPaginationService> _paginationServiceMock;
        private Mock<IEmailSenderService> _emailSenderServiceMock;
        private WishListService _service;

        private User _currentUser;
        private Book _book;
        private Wish _wish;
        private List<Wish> _wishes;

        [OneTimeSetUp]
        public void InitializeClass()
        {
            _wishRepositoryMock = new Mock<IRepository<Wish>>();
            _bookRepositoryMock = new Mock<IRepository<Book>>();
            _userResolverServiceMock = new Mock<IUserResolverService>();
            _paginationServiceMock = new Mock<IPaginationService>();
            _emailSenderServiceMock = new Mock<IEmailSenderService>();
            _service = new WishListService(
                _userResolverServiceMock.Object,
                _paginationServiceMock.Object,
                _emailSenderServiceMock.Object,
                _wishRepositoryMock.Object,
                _bookRepositoryMock.Object);

            MockData();
        }

        [SetUp]
        public void InitializeTest()
        {
            _wishRepositoryMock.Invocations.Clear();
        }

        [Test]
        public async Task GetWishesOfCurrentUser_NoExceptionsWasThrown_ReturnsPaginatedBookGetDto()
        {
            var pageableParams = new PageableParams();

            await _service.GetWishesOfCurrentUserAsync(pageableParams);

            _userResolverServiceMock.Verify(obj => obj.GetUserId());
            _wishRepositoryMock.Verify(obj => obj.GetAll());
            _paginationServiceMock.Verify(obj => obj.GetPageAsync<BookGetDto, Book>(It.IsAny<IQueryable<Book>>(), pageableParams));
        }

        [Test]
        public void AddWish_NoBookWithPassedIdInDatabase_ThrowsObjectNotFoundException()
        {
            _bookRepositoryMock.Setup(obj => obj.FindByIdAsync(_book.Id))
                .ReturnsAsync(value: null);

            _service.Invoking(s => s.AddWishAsync(_book.Id))
                .Should()
                .Throw<ObjectNotFoundException>();
        }

        [Test]
        public void AddWish_CurrentUserIdAndBookOwnerIdIsEqual_ThrowsInvalidOperationException()
        {
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(_currentUser.Id);
            _bookRepositoryMock.Setup(obj => obj.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Book { UserId = _currentUser.Id });

            _service.Invoking(s => s.AddWishAsync(It.IsAny<int>()))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Test]
        public async Task AddWish_NoExceptionsWasThrown_WishShouldBeAddedToDatabase()
        {
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(_currentUser.Id);
            _bookRepositoryMock.Setup(obj => obj.FindByIdAsync(_book.Id))
                .ReturnsAsync(_book);

            await _service.AddWishAsync(_book.Id);

            _wishRepositoryMock.Verify(
                obj => obj.Add(
                    It.Is<Wish>(w => w.UserId == _currentUser.Id && w.BookId == _book.Id)),
                Times.Once);
            _wishRepositoryMock.Verify(obj => obj.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void RemoveWish_NoBookWithPassedIdInWishListOfUser_ThrowsInvalidOperationException()
        {
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(_currentUser.Id);
            _wishRepositoryMock.Setup(obj => obj.FindByIdAsync(_currentUser.Id, _book.Id))
                .ReturnsAsync(value: null);

            _service.Invoking(s => s.RemoveWishAsync(_book.Id))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Test]
        public async Task RemoveWish_NoExceptionWasThrown_WishShouldBeRemovedFromDatabase()
        {
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(_currentUser.Id);
            _wishRepositoryMock.Setup(obj => obj.FindByIdAsync(_currentUser.Id, _book.Id))
                .ReturnsAsync(_wish);

            await _service.RemoveWishAsync(_book.Id);

            _wishRepositoryMock.Verify(obj => obj.Remove(_wish), Times.Once);
            _wishRepositoryMock.Verify(obj => obj.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CheckIfBookInWishListAsync_WishRepositoryReturnsNull_ReturnsFalse()
        {
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(_currentUser.Id);
            _wishRepositoryMock.Setup(obj => obj.FindByIdAsync(_currentUser.Id, _book.Id))
                .ReturnsAsync(value: null);

            var result = await _service.CheckIfBookInWishListAsync(_book.Id);

            _userResolverServiceMock.Verify(obj => obj.GetUserId());
            _wishRepositoryMock.Verify(obj => obj.FindByIdAsync(_currentUser.Id, _book.Id));

            result.Should().Be(false);
        }

        [Test]
        public async Task CheckIfBookInWishListAsync_WishRepositoryReturnsWishObject_ReturnsTrue()
        {
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(_currentUser.Id);
            _wishRepositoryMock.Setup(obj => obj.FindByIdAsync(_currentUser.Id, _book.Id))
                .ReturnsAsync(new Wish());

            var result = await _service.CheckIfBookInWishListAsync(_book.Id);

            _userResolverServiceMock.Verify(obj => obj.GetUserId());
            _wishRepositoryMock.Verify(obj => obj.FindByIdAsync(_currentUser.Id, _book.Id));

            result.Should().Be(true);
        }

        [Test]
        public async Task NotifyAboutAvailableBookAsync_ShouldNotifyUserWithBookInWishListAndAllowedEmail()
        {
            var userFullName = $"{_currentUser.FirstName} {_currentUser.LastName}".Trim();
            _wishRepositoryMock.Setup(obj => obj.GetAll()).Returns(_wishes.AsQueryable());

            await _service.NotifyAboutAvailableBookAsync(_book.Id);

            _emailSenderServiceMock.Verify(obj => obj.SendForWishBecameAvailable(
                userFullName,
                _book.Id,
                _book.Name,
                _currentUser.Email));
        }

        private void MockData()
        {
            _currentUser = new User
            {
                Id = 1,
                FirstName = "User",
                LastName = "Userovich",
                Email = "user@gmail.com",
                IsEmailAllowed = true
            };
            var userWithEmailNotAllowed = new User
            {
                Id = 2,
                FirstName = "Customer",
                LastName = "Customerovich",
                Email = "customer@gmail.com",
                IsEmailAllowed = false
            };

            _book = new Book
            {
                Id = 1,
                Name = "History of Test",
                UserId = It.Is<int>(id => id != _currentUser.Id)
            };
            var otherBook = new Book
            {
                Id = 2,
                Name = "History of Code",
                UserId = It.Is<int>(id => id != _currentUser.Id)
            };

            _wish = new Wish
            {
                UserId = _currentUser.Id,
                User = _currentUser,
                BookId = _book.Id,
                Book = _book
            };
            _wishes = new List<Wish>
            {
                _wish,
                new Wish { BookId = _book.Id, Book = _book, UserId = userWithEmailNotAllowed.Id, User = userWithEmailNotAllowed },
                new Wish { BookId = otherBook.Id, Book = otherBook, UserId = _currentUser.Id, User = _currentUser },
                new Wish { BookId = otherBook.Id, Book = otherBook, UserId = userWithEmailNotAllowed.Id, User = userWithEmailNotAllowed },
            };
        }
    }
}