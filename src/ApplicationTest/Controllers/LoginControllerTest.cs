using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using System.Threading.Tasks;
using Moq;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using BookCrossingBackEnd.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace ApplicationTest
{
    [TestFixture]
    class LoginControllerTest
    {
        [Test]
        public async Task LoginTest()
        {
            //Arrange
            var logDto = new LoginDto();
            var serviceMock = new Mock<IUserService>();
            var tokenMock = new Mock<ITokenService>();
            var expected = new { token = "token", FirstName = "first", LastName = "Last" };

            serviceMock.Setup(s => s.VerifyUserCredentials(It.IsAny<LoginDto>()))
                .ReturnsAsync(new UserDto() { FirstName = "first", LastName = "Last" });
            tokenMock.Setup(t => t.GenerateJSONWebToken(It.IsAny<UserDto>())).Returns("token");

            var controller = new LoginController(serviceMock.Object, tokenMock.Object);

            //Act

            var result = await controller.Login(logDto);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

    }
}