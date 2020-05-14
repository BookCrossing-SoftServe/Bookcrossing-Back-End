using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Email;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.AspNetCore.Hosting;
using FluentAssertions;
using Infrastructure.RDBMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Services
{
    [TestFixture]
    public class UsersServiceTests
    {
        private BookCrossingContext _context;
        private UsersService _usersService;
        private Mock<IEmailSenderService> _emailSenderServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IRepository<User>> _userRepositoryMock;
        private Mock<IRepository<ResetPassword>> _resetPasswordRepositoryMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _resetPasswordRepositoryMock = new Mock<IRepository<ResetPassword>>();
            _mapperMock = new Mock<IMapper>();
            _emailSenderServiceMock = new Mock<IEmailSenderService>();
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
            var options = new DbContextOptionsBuilder<BookCrossingContext>().UseInMemoryDatabase(databaseName: "Fake DB").ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _context = new BookCrossingContext(options);
            _usersService = new UsersService(_userRepositoryMock.Object, _mapper,_emailSenderServiceMock.Object, _resetPasswordRepositoryMock.Object);
        }
        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock.Reset();
        }
        [Test]
        public async Task GetById_UserExists_Returns_UserDto()
        {
            var usersMock = GetTestUsers().AsQueryable().BuildMock();
            _userRepositoryMock.Setup(s => s.GetAll()).Returns(usersMock.Object);

            var usersResult = await _usersService.GetById(x=>x.Id == 1);

            usersResult.Should().BeOfType<UserDto>();
            usersResult.Id.Should().Be(1);
        }
        private List<User> GetTestUsers()
        {
            return new List<User>
            {
                new User(){ Id = 1},
                new User(){ Id = 2}
            };
        }


    }
}
