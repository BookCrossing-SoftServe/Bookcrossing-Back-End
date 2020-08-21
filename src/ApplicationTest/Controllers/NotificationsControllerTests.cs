using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    internal class NotificationsControllerTests
    {
        private Mock<INotificationsService> _notificationsServiceMock;
        private NotificationsController _controller;

        [OneTimeSetUp]
        public void InitializeClass()
        {
            _notificationsServiceMock = new Mock<INotificationsService>();
            _controller = new NotificationsController(_notificationsServiceMock.Object);
        }

        [SetUp]
        public void InitializeTest()
        {
            _notificationsServiceMock.Invocations.Clear();
        }

        [Test]
        public async Task GetAll_ReturnsOkObjectResultWithNotificationDtos()
        {
            var notifications = new List<NotificationDto>();
            _notificationsServiceMock.Setup(obj => obj.GetAllForCurrentUserAsync())
                .ReturnsAsync(notifications);

            var result = await _controller.GetAll();

            _notificationsServiceMock.Verify(obj => obj.GetAllForCurrentUserAsync());

            result.Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Subject.Value
                .Should()
                .Be(notifications);
        }

        [Test]
        public async Task MarkAsRead_ServiceMethodThrowsInvalidOperationException_ReturnsForbiddenStatusCodeWishMessage()
        {
            var exceptionMessage = "User cannot mark as read notification that does not belong to him";
            _notificationsServiceMock.Setup(obj => obj.MarkAsReadAsync(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException(exceptionMessage));

            var result = await _controller.MarkAsRead(It.IsAny<int>());

            var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(403);
            objectResult.Value.Should().Be(exceptionMessage);
        }

        [Test]
        public async Task MarkAsRead_NoExceptionsWereThrown_ShouldCallServiceMethodToMarkNotificationAsReadAndReturnOkResult()
        {
            const int notificationId = 1;

            var result = await _controller.MarkAsRead(notificationId);

            _notificationsServiceMock.Verify(obj => obj.MarkAsReadAsync(notificationId));

            result
                .Should()
                .BeOfType<OkResult>();
        }

        [Test]
        public async Task MarkAllAsRead_ShouldCallServiceMethodToMarkAllNotificationsAndReturnOkResult()
        {
            var result = await _controller.MarkAllAsRead();

            _notificationsServiceMock.Verify(obj => obj.MarkAllAsReadForCurrentUserAsync());

            result
                .Should()
                .BeOfType<OkResult>();
        }

        [Test]
        public async Task Remove_ServiceMethodThrowsInvalidOperationException_ReturnsForbiddenStatusCodeWishMessage()
        {
            var exceptionMessage = "User cannot delete notification that does not belong to him";
            _notificationsServiceMock.Setup(obj => obj.RemoveAsync(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException(exceptionMessage));

            var result = await _controller.Remove(It.IsAny<int>());

            var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(403);
            objectResult.Value.Should().Be(exceptionMessage);
        }

        [Test]
        public async Task Remove_NoExceptionsWereThrown_ShouldCallServiceMethodToRemoveNotificationAsReadAndReturnOkResult()
        {
            const int notificationId = 1;

            var result = await _controller.Remove(notificationId);

            _notificationsServiceMock.Verify(obj => obj.RemoveAsync(notificationId));

            result
                .Should()
                .BeOfType<OkResult>();
        }

        [Test]
        public async Task RemoveAll_ShouldCallServiceMethodToMarkAllNotificationsAndReturnOkResult()
        {
            var result = await _controller.RemoveAll();

            _notificationsServiceMock.Verify(obj => obj.RemoveAllForCurrentUserAsync());

            result
                .Should()
                .BeOfType<OkResult>();
        }
    }
}
