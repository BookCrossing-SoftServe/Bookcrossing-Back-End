using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTest.ContollerTests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUserResolverService> _mockUserResolverService;
        private Mock<IUserService> _mockUserService;
        private UsersController _usersController;

        [SetUp]
        public void Init()
        {
            _mockUserResolverService = new Mock<IUserResolverService>();
            _mockUserService = new Mock<IUserService>();
            _usersController = new UsersController(_mockUserService.Object, _mockUserResolverService.Object);
        }

        #region api/users [get] tests



        [Test]
        public async Task Get_ReturnsAViewResult_WithAListOfUsers()
        {
            //arrange
            
            _mockUserService.Setup(p => p.GetAllUsers())
                .ReturnsAsync(GetTestUsers);
            //act
            var result = await _usersController.Get();

            //assert
            var viewResult = result.Should().BeOfType<OkObjectResult>();
            var jsonResult = viewResult.Subject.Value.Should().BeOfType<List<UserDto>>();
            jsonResult.Subject.Count.Should().Be(5);
        }
        [Test]
        public async Task Get_ReturnsANoContentResult_WhenThereIsNoUsers()
        {
            //arrange
            
            _mockUserService
                .Setup(p => p.GetAllUsers())
                .ReturnsAsync((List<UserDto>)null);
            //act
            var result = await _usersController.Get();
            //assert
            var viewResult = result.Should().BeOfType<NoContentResult>();
        }

        #endregion


        #region api/users/{id} [put] tests
        [TestCase(false,1)]
        [TestCase(false,5)]
        [TestCase(false,8)]
        public async Task Update_ReturnsForbidden_UserWantsChangeNotHisProfileAndUserIsNotAdmin(bool userIsAdmin,int idFromCredential)
        {
            //arrange
            
            _mockUserResolverService.Setup(p => p.IsUserAdmin()).Returns(userIsAdmin);
            _mockUserResolverService.Setup(p => p.GetUserId()).Returns(idFromCredential);


            var updatingUser = new UserUpdateDto()
            {
                Email = "test@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                RoleId = 2

            };
            //act
            var result = await _usersController.Update(2, updatingUser);

            //assert
            result.Should().BeOfType<ForbidResult>();
        }

        [TestCase(2,2,false)]
        [TestCase(1,1,false)]
        [TestCase(5,5,false)]
        [TestCase(2,5,true)]
        public async Task Update_ReturnsNotContent204_UserChangesHisProfileOrUserIsAdmin(int idFromRoute,int idFromCredentials,bool userIsAdmin)
        {
            //arrange
            _mockUserResolverService.Setup(p => p.IsUserAdmin()).Returns(userIsAdmin);
            _mockUserResolverService.Setup(p => p.GetUserId()).Returns(idFromCredentials);

            var updatingUser = new UserUpdateDto()
            {
                Email = "test@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                RoleId = 2

            };

            //act
            var result = await _usersController.Update(idFromRoute, updatingUser);
            
            //assert
            result.Should().BeOfType<NoContentResult>();
        }


        #endregion

        public List<UserDto> GetTestUsers()
        {
            List<UserDto> userDtos = new List<UserDto>(5);
            userDtos.Add(new UserDto()
            {
                Email = "vitaliy2001rudenko@gmail.com",
                FirstName = "Vitaliy",
                Id = 1,
                LastName = "Rudenko",
                MiddleName = "Stepanovich"
                

            });
            userDtos.Add(new UserDto()
            {
                Email = "romanferenc@gmail.com",
                FirstName = "roman",
                Id = 2,
                LastName = "Ferench",
                MiddleName = "Ivanovich"


            });
            userDtos.Add(new UserDto()
            {
                Email = "volodyakorpal@gmail.com",
                FirstName = "Volodya",
                Id = 3,
                LastName = "Korpal",
                MiddleName = "Stepanovich"


            });
            userDtos.Add(new UserDto()
            {
                Email = "test@gmail.com",
                FirstName = "test",
                Id = 4,
                LastName = "test",
                MiddleName = "test"


            });
            userDtos.Add(new UserDto()
            {
                Email = "natalyarevutska@gmail.com",
                FirstName = "Natalya",
                Id = 5,
                LastName = "Revytska",
                MiddleName = "?"


            });
            return userDtos;
        }
    }

}
