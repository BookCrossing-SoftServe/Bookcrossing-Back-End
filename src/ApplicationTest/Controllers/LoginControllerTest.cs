using NUnit.Framework;
using Application.Dto;
using System.Threading.Tasks;
using Moq;
using Application.Services.Interfaces;
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
            var tokenMock = new Mock<ITokenService>();

            tokenMock.Setup(s => s.VerifyUserCredentials(It.IsAny<LoginDto>()))
                .ReturnsAsync(new UserDto() { FirstName = "first", LastName = "Last" });
            tokenMock.Setup(t => t.GenerateJSONWebToken(It.IsAny<UserDto>())).Returns("token");            

            var controller = new LoginController( tokenMock.Object);

            //Act
            var result = await controller.Login(logDto);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

    }
}