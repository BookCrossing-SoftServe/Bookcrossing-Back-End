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
            var bookId = 1;
            _bookRepositoryMock.Setup(obj => obj.FindByIdAsync(bookId))
                .ReturnsAsync(value: null);

            _service.Invoking(s => s.AddWishAsync(bookId))
                .Should()
                .Throw<ObjectNotFoundException>()
                .WithMessage($"There is no book with id = {bookId} in database");
        }

        [Test]
        public void AddWish_CurrentUserIdAndBookOwnerIdIsEqual_ThrowsInvalidOperationException()
        {
            var currentUserId = 1;
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(currentUserId);
            _bookRepositoryMock.Setup(obj => obj.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Book { UserId = currentUserId });

            _service.Invoking(s => s.AddWishAsync(It.IsAny<int>()))
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("User cannot add his book to wish list");
        }

        [Test]
        public async Task AddWish_NoExceptionsWasThrown_WishShouldBeAddedToDatabase()
        {
            var currentUserId = 1;
            var book = new Book
            {
                Id = 1,
                UserId = It.Is<int>(x => x != currentUserId)
            };
            var wish = new Wish
            {
                UserId = currentUserId,
                BookId = book.Id
            };
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(currentUserId);
            _bookRepositoryMock.Setup(obj => obj.FindByIdAsync(book.Id))
                .ReturnsAsync(book);

            await _service.AddWishAsync(book.Id);

            _wishRepositoryMock.Verify(
                obj => obj.Add(
                    It.Is<Wish>(w => w.UserId == currentUserId && w.BookId == book.Id)),
                Times.Once);
            _wishRepositoryMock.Verify(obj => obj.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void RemoveWish_NoBookWithPassedIdInWishListOfUser_ThrowsInvalidOperationException()
        {
            var currentUserId = 1;
            var bookId = 1;
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(currentUserId);
            _wishRepositoryMock.Setup(obj => obj.FindByIdAsync(currentUserId, bookId))
                .ReturnsAsync(value: null);

            _service.Invoking(s => s.RemoveWishAsync(bookId))
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(
                    $"Cannot delete book with id = {bookId} from current user's wish list, because there is no book with id = {bookId}");
        }

        [Test]
        public async Task RemoveWish_NoExceptionWasThrown_WishShouldBeRemovedFromDatabase()
        {
            var currentUserId = 1;
            var bookId = 1;
            var wish = new Wish
            {
                UserId = currentUserId,
                BookId = bookId
            };
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(currentUserId);
            _wishRepositoryMock.Setup(obj => obj.FindByIdAsync(currentUserId, bookId))
                .ReturnsAsync(wish);

            await _service.RemoveWishAsync(bookId);

            _wishRepositoryMock.Verify(obj => obj.Remove(wish), Times.Once);
            _wishRepositoryMock.Verify(obj => obj.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CheckIfBookInWishListAsync_WishRepositoryReturnsNull_ReturnsFalse()
        {
            var userId = 1;
            var bookId = 1;
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(userId);
            _wishRepositoryMock.Setup(obj => obj.FindByIdAsync(userId, bookId))
                .ReturnsAsync(value: null);

            var result = await _service.CheckIfBookInWishListAsync(bookId);

            _userResolverServiceMock.Verify(obj => obj.GetUserId());
            _wishRepositoryMock.Verify(obj => obj.FindByIdAsync(userId, bookId));

            result.Should().Be(false);
        }

        [Test]
        public async Task CheckIfBookInWishListAsync_WishRepositoryReturnsWishObject_ReturnsTrue()
        {
            var userId = 1;
            var bookId = 1;
            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(userId);
            _wishRepositoryMock.Setup(obj => obj.FindByIdAsync(userId, bookId))
                .ReturnsAsync(new Wish());

            var result = await _service.CheckIfBookInWishListAsync(bookId);

            _userResolverServiceMock.Verify(obj => obj.GetUserId());
            _wishRepositoryMock.Verify(obj => obj.FindByIdAsync(userId, bookId));

            result.Should().Be(true);
        }

        [Test]
        public async Task NotifyAboutAvailableBookAsync_ShouldNotifyUserWithBookInWishListAndAllowedEmail()
        {
            var bookId = 1;
            var book = new Book { Id = bookId, Name = "Name" };
            var userId = 1;
            var userWithEmailAllowed = new User { Id = 1, FirstName = "User1", LastName = "User1", IsEmailAllowed = true, Email = "user@mail.com"};
            var userFullName = userWithEmailAllowed.FirstName + " " + userWithEmailAllowed.LastName;
            var userWishEmailNotAllowed = new User { IsEmailAllowed = false };
            var wishes = new List<Wish>
            {
                new Wish {BookId = 1, Book = book, UserId = userId, User = userWithEmailAllowed},
                new Wish {BookId = 1, Book = book, User = userWishEmailNotAllowed},
                new Wish {BookId = 2, UserId = userId, User = userWithEmailAllowed},
                new Wish {BookId = 2, User = userWishEmailNotAllowed},
            };
            _wishRepositoryMock.Setup(obj => obj.GetAll()).Returns(wishes.AsQueryable());

            await _service.NotifyAboutAvailableBookAsync(bookId);

            _emailSenderServiceMock.Verify(obj => obj.SendForWishBecameAvailable(
                userFullName, 
                bookId, 
                book.Name, 
                userWithEmailAllowed.Email));
        }
    }
}