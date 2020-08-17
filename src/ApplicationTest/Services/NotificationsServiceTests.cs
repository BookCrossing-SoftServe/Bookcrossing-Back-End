using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Services
{
    [TestFixture]
    class NotificationsServiceTests
    {
        private Mock<IHubContext<NotificationsHub>> _notificationsHubContextMock;
        private Mock<IUserResolverService> _userResolverServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IRepository<Notification>> _notificationRepositoryMock;
        private INotificationsService _service;

        [OneTimeSetUp]
        public void InitializeClass()
        {
            _notificationsHubContextMock = new Mock<IHubContext<NotificationsHub>>();
            _userResolverServiceMock = new Mock<IUserResolverService>();
            _mapperMock = new Mock<IMapper>();
            _notificationRepositoryMock = new Mock<IRepository<Notification>>();
            _service = new NotificationsService(
                _notificationRepositoryMock.Object,
                _userResolverServiceMock.Object,
                _mapperMock.Object,
                _notificationsHubContextMock.Object);
        }

        [Test]
        public async Task NotifyAsync_ShouldAddNotificationToDatabaseAndSendItViaNubToFrontend()
        {
            var user = new User
            {
                Id = 1,
                Email = "user@mail.com"
            };
            var message = "Some message";
            var bookId = 1;
            var action = NotificationAction.Open;
            var notification = new Notification
            {
                UserId = user.Id,
                Message = message,
                Action = action,
                BookId = bookId
            };
            var notificationDto = new NotificationDto
            {
                Message = message,
                Action = action,
                BookId = bookId
            };
            _mapperMock.Setup(
                obj => obj.Map<NotificationDto>(It.Is<Notification>(x =>
                x.UserId == notification.UserId && x.Message == notification.Message &&
                x.Action == notification.Action && x.BookId == notification.BookId)))
                .Returns(notificationDto);

            await _service.NotifyAsync(user, message, bookId, action);

            _notificationRepositoryMock.Verify(
                obj => obj.Add(It.Is<Notification>(x =>
                    x.UserId == notification.UserId && x.Message == notification.Message &&
                    x.Action == notification.Action && x.BookId == notification.BookId)),
                Times.Once);
            _notificationRepositoryMock.Verify(obj => obj.SaveChangesAsync());
            _notificationsHubContextMock.Verify(obj =>
                obj.Clients.User(user.Email)
                    .SendAsync("Notify", notificationDto, default), Times.Once);
        }
    }
}
