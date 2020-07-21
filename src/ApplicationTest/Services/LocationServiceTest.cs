using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
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
    public class LocationServiceTests
    {
        private BookCrossingContext _context;
        private LocationService _locationService;
        private Mock<IRepository<Location>> _locationRepositoryMock;
        private Mock<IPaginationService> _paginationServiceMock;
        private Mock<IMapper> _mapperMock;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _locationRepositoryMock = new Mock<IRepository<Location>>();
            _mapperMock = new Mock<IMapper>();
            _paginationServiceMock = new Mock<IPaginationService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.MapperProfilers.LocationProfile());
                mc.AddProfile(new Application.MapperProfilers.UserProfile());
            });
            var _mapper = mappingConfig.CreateMapper();
            var options = new DbContextOptionsBuilder<BookCrossingContext>().UseInMemoryDatabase(databaseName: "Fake DB").ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _context = new BookCrossingContext(options);
            _locationService = new LocationService(_locationRepositoryMock.Object, _mapper, _paginationServiceMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _locationRepositoryMock.Reset();
        }

        private List<Location> GetTestLocations()
        {
            return new List<Location>
            {
                new Location()
                {
                    Id = 1,
                    City = "Lviv",
                    IsActive = true,
                    Street = "Panasa Myrnogo",
                    OfficeName = "HQ",
                    UserRoom = new List<UserRoom>(){new UserRoom(){ Id = 1, LocationId = 1, RoomNumber = "33" } }
                },
                new Location()
                {
                    Id = 2,
                    City = "Lviv",
                    IsActive = true,
                    Street = "Gorodotska",
                    OfficeName = "1",
                    UserRoom = new List<UserRoom>(){new UserRoom(){ Id = 2, LocationId = 2, RoomNumber = "1" } }
                },
            };
        }

        [Test]
        public async Task GetLocationById_LocationExists_ReturnsLocationDto()
        {
            int locationId = 1;
            var locationsMock = GetTestLocations().AsQueryable().BuildMock();
            _locationRepositoryMock.Setup(s => s.GetAll()).Returns(locationsMock.Object);

            var locationResult = await _locationService.GetById(locationId);

            locationResult.Should().BeOfType<LocationDto>();
            locationResult.Id.Should().Be(1);
        }

        [Test]
        public async Task GetAllLocations_LocationsExist_ReturnsListOfLocationDtos()
        {
            int expectedCount = 2;
            var expectedLocation = new LocationDto()
            {
                Id = 1,
                City = "Lviv",
                IsActive = true,
                Street = "Panasa Myrnogo",
                OfficeName = "HQ",
                Rooms = new List<string>() { "33" },
            };
            var locationsMock = GetTestLocations().AsQueryable().BuildMock();
            _locationRepositoryMock.Setup(s => s.GetAll()).Returns(locationsMock.Object);

            var locationResult = await _locationService.GetAll();

            locationResult.Should().BeOfType<List<LocationDto>>();
            locationResult.Count.Should().Be(expectedCount);
            locationResult.Find(x => x.Id == 1 && x.City == "Lviv" && x.OfficeName == "HQ").Should().NotBeNull();
        }

        [Test]
        public async Task RemoveLocation_LocationExists_ReturnsLocationDtoRemoved()
        {
            int locationId = 1;
            var existingLocation = new Location()
            {
                Id = 1,
                City = "Lviv",
                IsActive = true,
                Street = "Panasa Myrnogo",
                OfficeName = "HQ",
                UserRoom = new List<UserRoom>() { new UserRoom() { Id = 2, LocationId = 2, RoomNumber = "1" } }
            };
            _locationRepositoryMock.Setup(s => s.FindByIdAsync(locationId)).ReturnsAsync(existingLocation);

            var locationResult = await _locationService.Remove(locationId);

            locationResult.Should().BeOfType<LocationDto>();
            locationResult.Id.Should().Be(1);
            locationResult.Street.Should().Be("Panasa Myrnogo");
        }

        [Test]
        public async Task RemoveLocation_LocationNotExist_ReturnsNull()
        {
            int locationId = 1;
            _locationRepositoryMock.Setup(s => s.FindByIdAsync(locationId)).ReturnsAsync(null as Location);

            var locationResult = await _locationService.Remove(locationId);

            locationResult.Should().BeNull();
        }
    }
}
