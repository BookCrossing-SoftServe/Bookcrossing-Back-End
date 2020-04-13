using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using AutoMapper;
using Domain.RDBMS.Entities;
using NUnit.Framework;
using
using Moq;
using Domain.RDBMS;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
       

        [Test]
        public async Task VerifyUserCredentials_UserProvidedRightCredentials_ReturnsUser()
        {
            var users = new List<User>
            {
                new User {Password = "1", Email = "1"},
                new User {Password = "usedrugs3", Email = "vit.hack"}
            };

            var loginDto = new LoginDto
            {
                Email = "vit.hack",
                Password = "usedrugs3"
            };


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mp => mp.Map<User, UserDto>(It.IsAny<User>()))
                .Returns((User ser) => new UserDto() { Email = ser.Email });

            var repositoryMock = new Mock<IRepository<User>>();
            repositoryMock.Setup(p => p.GetAll()).Returns(users.AsQueryable());



            var userService = new UsersService(repositoryMock.Object, mapperMock.Object);




            var result =
                await userService.VerifyUserCredentials(loginDto);



            Assert.IsNotNull(result);
        }
    }
}