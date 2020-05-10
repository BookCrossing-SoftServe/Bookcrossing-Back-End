using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.ServicesTests
{
    [TestFixture]
    internal class TokenServiceTests
    {
        private TokenService _usersService;
      
        private Mock<IRepository<User>> _userRepositoryMoq;
        private Mock<IConfiguration> _configurationMoq;
        [SetUp]
        public void Init()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.MapperProfilers.AuthorProfile());
                mc.AddProfile(new Application.MapperProfilers.BookChildCommentProfile());
                mc.AddProfile(new Application.MapperProfilers.BookRootCommentProfile());
                mc.AddProfile(new Application.MapperProfilers.GenreProfile());
                mc.AddProfile(new Application.MapperProfilers.LocationProfile());
                mc.AddProfile(new Application.MapperProfilers.RequestProfile());
                mc.AddProfile(new Application.MapperProfilers.UserProfile());
                mc.AddProfile(new Application.MapperProfilers.BookProfile());
            });
            var _mapper = mappingConfig.CreateMapper();
            _userRepositoryMoq = new Mock<IRepository<User>>();
            _configurationMoq = new Mock<IConfiguration>();
            _usersService = new TokenService(_configurationMoq.Object, _mapper, _userRepositoryMoq.Object);
        }

        [Test]
        public async Task VerifyUserCredentials_UserProvidedRightCredentials_ReturnsUserDto()
        {
            #region arrange
            var loginModel = new LoginDto()
            {
                Password = "usedrugs3",
                Email = "vitaliy2001rudenko@gmail.com"
            };

            var usersMoq = GetTestUsers().AsQueryable().BuildMock();
            _userRepositoryMoq.Setup(p => p.GetAll()).Returns(usersMoq.Object);
            #endregion

            #region act
            var result = await _usersService.VerifyUserCredentials(loginModel);
            #endregion

            #region assert
            result.Should().NotBeNull();
            #endregion
        }

        [Test]
        public void VerifyUserCredentials_UserProvidedWrongCredentials_ThrowsInvalidCredentialsException()
        {

            #region arrange
            var loginModel = new LoginDto()
            {
                Password = "usedrfdrugs3",
                Email = "vitaliy2001rudenko@gmail.com"
            };

            var usersMoq = GetTestUsers().AsQueryable().BuildMock();
            _userRepositoryMoq.Setup(p => p.GetAll()).Returns(usersMoq.Object);
            #endregion

            #region act
            Func<Task> f = async () => { await _usersService.VerifyUserCredentials(loginModel); };
            #endregion

            #region assert
            f.Should().Throw<InvalidCredentialException>();
            #endregion
        }

        [Test]
        public void GenerateJSONWebToken_ReturnsTokenString()
        {
            #region arrange
            _configurationMoq.Setup(x => x["Jwt: Issuer"]).Returns("BookCrossing.com");
            _configurationMoq.Setup(x => x["Jwt:Key"]).Returns("SoftServeBookCrossingSecretKey");

            UserDto testUserDto = new UserDto()
            {
                Id=1,
                Email="vitaliy2001rudenko@gmail.com",
                Role = new Role
                {
                    Id=1,
                    Name="user"
                }

            };
            #endregion

            #region act
            var result = _usersService.GenerateJSONWebToken(testUserDto);
            #endregion

            #region assert

            result.Should().MatchRegex("^[A-Za-z0-9-_=]+\\.[A-Za-z0-9-_=]+\\.?[A-Za-z0-9-_.+/=]*$");
            #endregion


        }


        public List<User> GetTestUsers()
        {
            List<User> users = new List<User>();
            users.Add(
                new User
                {
                    Id = 1,
                    Email = "vitaliy2001rudenko@gmail.com",
                    Password = "usedrugs3"
                });

            users.Add(new User
            {
                       Id=2,
                       Email="natalya_revurska@gmail.com",
                       Password="natalya_best_mentor"
            });
            users.Add(new User
            {
                Id = 3,
                Email = "volodya1999korpal@gmail.com",
                Password = "123weakpassword"
            });
            users.Add(new User
            {
                Id = 4,
                Email = "mychalio@mail.ru",
                Password = "e1p2a3m3"
            });
            return users;

        }

    }

}
