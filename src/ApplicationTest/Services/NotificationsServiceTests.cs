using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Application.SignalR.Hubs;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Services
{
    [TestFixture]
    internal class NotificationsServiceTests
    {
        private Mock<IRepository<Notification>> _notificationsRepositoryMock;
        private Mock<IUserResolverService> _userResolverServiceMock;
        private Mock<IHubContext<NotificationsHub>> _notificationsHubContextMock;
        private Mock<IHubClients> _hubClientsMock;
        private Mock<IMapper> _mapperMock;
        private INotificationsService _service;

        private string _methodNameOfHubProxy;

        private User _currentUser;
        private Notification _notification;
        private List<Notification> _notifications;
        private NotificationDto _notificationDto;
        private List<NotificationDto> _notificationDtos;

        [OneTimeSetUp]
        public void InitializeClass()
        {
            _notificationsRepositoryMock = new Mock<IRepository<Notification>>();
            _userResolverServiceMock = new Mock<IUserResolverService>();
            _mapperMock = new Mock<IMapper>();
            _notificationsHubContextMock = new Mock<IHubContext<NotificationsHub>>();
            _hubClientsMock = new Mock<IHubClients>();
            _service = new NotificationsService(
                _notificationsRepositoryMock.Object,
                _userResolverServiceMock.Object,
                _mapperMock.Object,
                _notificationsHubContextMock.Object);
            _methodNameOfHubProxy = "Notify";

            MockData();

            _userResolverServiceMock.Setup(obj => obj.GetUserId())
                .Returns(_currentUser.Id);
            _notificationsRepositoryMock.Setup(obj => obj.GetAll())
                .Returns(_notifications.AsQueryable().BuildMock().Object);
            _notificationsHubContextMock.SetupGet(obj => obj.Clients)
                .Returns(_hubClientsMock.Object);
        }

        [SetUp]
        public void InitializeTest()
        {
            _notificationsRepositoryMock.Invocations.Clear();
        }

        [Test]
        public async Task NotifyAsync_MessageArgumentIsNull_ThrowsArgumentNullException()
        {
            await _service.Invoking(obj => obj.NotifyAsync(It.IsAny<int>(), null))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [TestCase("")]
        [TestCase(" \n\t\r\v")]
        public async Task NotifyAsync_MessageArgumentIsEmptyOrWhiteSpaces_ThrowsArgumentException(string message)
        {
            await _service.Invoking(obj => obj.NotifyAsync(It.IsAny<int>(), message))
                .Should()
                .ThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task NotifyAsync_MessageArgumentIsNotNullOrWhiteSpaces_AddsNotificationToDatabaseAndSendsItToClientWithPassedUserId()
        {
            var clientProxyMock = new Mock<IClientProxy>();
            _hubClientsMock.Setup(obj => obj.User(_notification.UserId.ToString()))
                .Returns(clientProxyMock.Object);
            _mapperMock.Setup(obj => obj.Map<NotificationDto>(It.Is<Notification>(notification =>
                    notification.UserId == _notification.UserId && notification.Message == _notification.Message &&
                    notification.BookId == _notification.BookId && notification.Action == _notification.Action)))
                .Returns(_notificationDto);

            await _service.NotifyAsync(
                _notification.UserId,
                _notification.Message,
                _notification.BookId,
                _notification.Action);

            _notificationsRepositoryMock.Verify(
                obj => obj.Add(It.Is<Notification>(notification =>
                    notification.UserId == _notification.UserId && notification.Message == _notification.Message &&
                    notification.BookId == _notification.BookId && notification.Action == _notification.Action)),
                Times.Once);
            _notificationsRepositoryMock.Verify(obj => obj.SaveChangesAsync());
            clientProxyMock.Verify(
                obj => obj.SendCoreAsync(
                    _methodNameOfHubProxy,
                    new object[] { _notificationDto },
                    default),
                Times.Once);
        }

        [Test]
        public async Task GetAllForCurrentUserAsync_ReturnsNotificationsOfCurrentUser()
        {
            var expectedNotificationsEntities = _notifications
                .Where(notification => notification.UserId == _currentUser.Id)
                .OrderByDescending(notification => notification.CreatedAt)
                .ToList();
            var expectedNotificationsDtos = expectedNotificationsEntities
                .Select(entity => _notificationDtos.First(dto => dto.Id == entity.Id))
                .ToList();
            _mapperMock.Setup(
                    obj => obj.Map<IEnumerable<NotificationDto>>(
                            It.Is<IEnumerable<Notification>>(
                                notifications =>
                                    ListsHasSameElements(notifications.ToList(), expectedNotificationsEntities))))
                .Returns(expectedNotificationsDtos);

            var result = await _service.GetAllForCurrentUserAsync();

            result.Should().BeEquivalentTo(expectedNotificationsDtos);
        }

        [Test]
        public async Task MarkAsReadAsync_NotificationWithPassedIdDoesNotExist_ThrowsObjectNotFoundException()
        {
            _notificationsRepositoryMock.Setup(obj => obj.FindByIdAsync(_notification.Id))
                .ReturnsAsync(value: null);

            await _service.Invoking(obj => obj.MarkAsReadAsync(_notification.Id))
                .Should()
                .ThrowAsync<ObjectNotFoundException>();
        }

        [Test]
        public async Task MarkAsReadAsync_NotificationUserIdIsNotEqualToCurrentUserId_ThrowsInvalidOperationException()
        {
            var expectedNotification = _notifications.First(notification => notification.UserId != _currentUser.Id);
            _notificationsRepositoryMock.Setup(obj => obj.FindByIdAsync(expectedNotification.Id))
                .ReturnsAsync(expectedNotification);

            await _service.Invoking(obj => obj.MarkAsReadAsync(expectedNotification.Id))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task MarkAsReadAsync_NotificationExistsAndUserIdEqualsToCurrentUserId_SetIsReadPropertyToTrueForNotificationAndUpdatesDatabase()
        {
            _notificationsRepositoryMock.Setup(obj => obj.FindByIdAsync(_notification.Id))
                .ReturnsAsync(_notification);

            await _service.MarkAsReadAsync(_notification.Id);

            _notification.IsRead
                .Should()
                .BeTrue();
            _notificationsRepositoryMock.Verify(obj => obj.Update(_notification), Times.Once);
            _notificationsRepositoryMock.Verify(obj => obj.SaveChangesAsync());
        }

        [Test]
        public async Task MarkAllAsReadForCurrentUserAsync_SetIsReadPropertyToTrueForNotificationsWithUserIdEqualsToCurrentUserId()
        {
            await _service.MarkAllAsReadForCurrentUserAsync();

            _notifications
                .Where(notification => notification.UserId == _currentUser.Id)
                .Should()
                .OnlyContain(notification => notification.IsRead);
            _notificationsRepositoryMock.Verify(obj => obj.SaveChangesAsync());
        }

        [Test]
        public async Task RemoveAsync_NotificationWithPassedIdDoesNotExist_ThrowsObjectNotFoundException()
        {
            _notificationsRepositoryMock.Setup(obj => obj.FindByIdAsync(_notification.Id))
                .ReturnsAsync(value: null);

            await _service.Invoking(obj => obj.RemoveAsync(_notification.Id))
                .Should()
                .ThrowAsync<ObjectNotFoundException>();
        }

        [Test]
        public async Task RemoveAsync_NotificationUserIdIsNotEqualToCurrentUserId_ThrowsInvalidOperationException()
        {
            var expectedNotification = _notifications.First(notification => notification.UserId != _currentUser.Id);
            _notificationsRepositoryMock.Setup(obj => obj.FindByIdAsync(expectedNotification.Id))
                .ReturnsAsync(expectedNotification);

            await _service.Invoking(obj => obj.RemoveAsync(expectedNotification.Id))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RemoveAsync_NotificationExistsAndUserIdEqualsToCurrentUserId_ShouldRemoveNotificationFromDatabase()
        {
            _notificationsRepositoryMock.Setup(obj => obj.FindByIdAsync(_notification.Id))
                .ReturnsAsync(_notification);

            await _service.RemoveAsync(_notification.Id);

            _notificationsRepositoryMock.Verify(obj => obj.Remove(_notification), Times.Once);
            _notificationsRepositoryMock.Verify(obj => obj.SaveChangesAsync());
        }

        [Test]
        public async Task RemoveAllForCurrentUserAsync_RemoveNotificationsWithUserIdEqualsToCurrentUserId()
        {
            var notificationsToRemove = _notifications.Where(entity => entity.UserId == _currentUser.Id);

            await _service.RemoveAllForCurrentUserAsync();
            
            _notificationsRepositoryMock.Verify(obj => obj.RemoveRange(notificationsToRemove));
            _notificationsRepositoryMock.Verify(obj => obj.SaveChangesAsync());
        }

        private void MockData()
        {
            _currentUser = new User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test"
            };

            _notifications = new List<Notification>
            {
                new Notification
                {
                    Id = 1,
                    Message = "Test notification 1",
                    UserId = _currentUser.Id,
                    BookId = 1,
                    Action = NotificationAction.Open,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                },

                new Notification
                {
                    Id = 2,
                    Message = "Test notification 2",
                    UserId = _currentUser.Id,
                    BookId = 1,
                    Action = NotificationAction.Open,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                },

                new Notification
                {
                    Id = 3,
                    Message = "Test notification 3",
                    UserId = _currentUser.Id,
                    BookId = 1,
                    Action = NotificationAction.Open,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                },

                new Notification
                {
                    Id = 4,
                    Message = "Test notification 4",
                    UserId = 2,
                    BookId = 1,
                    Action = NotificationAction.Open,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                }
            };

            _notificationDtos = _notifications.Select(notification => new NotificationDto
            {
                Id = notification.Id,
                Message = notification.Message,
                IsSeen = notification.IsRead,
                BookId = notification.BookId,
                Action = notification.Action,
                Date = notification.CreatedAt
            }).ToList();

            _notification = _notifications.First();
            _notificationDto = _notificationDtos.First();
        }

        private bool ListsHasSameElements(List<Notification> obj1, List<Notification> obj2)
        {
            var tempList1 = obj1.Except(obj2).ToList();
            var tempList2 = obj2.Except(obj1).ToList();

            return !(tempList1.Any() || tempList2.Any());
        }
    }
}
