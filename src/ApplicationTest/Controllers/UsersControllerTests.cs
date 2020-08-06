using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Password;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

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

        [Test]
        public async Task ResetPassword_ValidUser_ReturnsIActionResultOK()
        {
            var resetPasswordDto = new ResetPasswordDto() {Email = "test@gmail.com"};
            _mockUserService.Setup(x => x.SendPasswordResetConfirmation(resetPasswordDto.Email))
                .Returns(Task.CompletedTask);
            var result = await _usersController.ForgotPassword(resetPasswordDto);
            result.Should().BeOfType<OkResult>();

        }

        [Test]
        public async Task ForgotPassword_ValidUser_ReturnsIActionResultOK()
        {

            var resetPasswordDto = new ResetPasswordDto() { Email = "test@gmail.com" };
            _mockUserService.Setup(x => x.SendPasswordResetConfirmation(resetPasswordDto.Email))
                .Returns(Task.CompletedTask);
            var result = await _usersController.CreateNewPassword(resetPasswordDto);
            result.Should().BeOfType<OkResult>();

        }

        [Test]
        [TestCase(201)]
        public async Task GetUserId_ReturnsActionResultInt(int credentialId)
        {
            //arrange

            _mockUserResolverService.Setup(p => p.GetUserId())
                .Returns(credentialId);
            //act
            var result = await _usersController.GetUserId();

            //assert
           result.Should().BeOfType<ActionResult<int>>();
           result.Result.Should().BeOfType<OkObjectResult>();
        }

        #region api/users [get] tests

        [Test]
        public async Task Get_WithAListOfUsers_ReturnsViewResultListOfUsers()
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
        public async Task GetUsers_WhenThereIsNoUsers_ReturnsNoContentResult()
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
        public async Task UpdateUser_UserWantsChangeNotHisProfileAndUserIsNotAdmin_ReturnsForbidden(bool userIsAdmin,int idFromCredential)
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
        public async Task UpdateUser_ChangesHisProfileUserIsAdmin__ReturnsNoContent204(int idFromRoute,int idFromCredentials,bool userIsAdmin)
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


        [Test]
        public async Task Register_UserSuccessfulRegistered_ReturnsCreatedAtActionResult()
        {
            var testRegisterDto = new RegisterDto()
            {
                Id = 0,
                Email = "test@gmail.com"
            };
            _mockUserService.Setup(m => m.AddUser(It.IsAny<RegisterDto>()))
                .ReturnsAsync(testRegisterDto);

            var result = await _usersController.Register(It.IsAny<RegisterDto>());

            result.Result.Should().BeOfType<CreatedAtActionResult>();
            (result.Result as CreatedAtActionResult).Value.Should()
                .BeEquivalentTo(testRegisterDto, 
                    opt => opt.Excluding(m => m.Id));
        }

        [Test]
        public async Task Register_Fail_ReturnsBadRequestResult()
        {
            _mockUserService.Setup(m => m.AddUser(It.IsAny<RegisterDto>()))
                .ReturnsAsync(value: null);

            var result = await _usersController.Register(It.IsAny<RegisterDto>());

            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Get_UserWasFound_ReturnsOkObjectResultWithUserDto()
        {
            var expectedResult = new UserDto() {Id = 1};
            _mockUserService.Setup(m => m.GetById(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(expectedResult);

            var result = await _usersController.Get(It.IsAny<int>());

            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<UserDto>>();
            result.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task Get_UserWasNotFound_ReturnsNotFoundResult()
        {
            _mockUserService.Setup(m => m.GetById(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(value: null);

            var result = await _usersController.Get(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Delete_AnyIssue_ReturnsOkResult()
        {
            var result = await _usersController.Delete(It.IsAny<int>());

            _mockUserService.Verify(m => m.RemoveUser(It.IsAny<int>()));
            result.Should().BeOfType<OkResult>();
        }

        private List<UserDto> GetTestUsers()
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
