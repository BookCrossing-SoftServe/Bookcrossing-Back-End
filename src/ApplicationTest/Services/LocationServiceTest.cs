using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Infrastructure.RDBMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MimeKit.Cryptography;
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

        private List<Location> _locations;
        private Mock<IQueryable<Location>> _locationsQueryableMock;
        private List<LocationDto> _locationsDto;
        private Location _location;
        private LocationDto _locationDto;

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
            _locationService = new LocationService(_locationRepositoryMock.Object, _mapperMock.Object, _paginationServiceMock.Object);

            MockData();
        }

        [SetUp]
        public void SetUp()
        {
            _locationRepositoryMock.Reset();
        }

        [Test]
        public async Task GetLocationById_LocationExists_ReturnsLocationDto()
        {
            _locationRepositoryMock.Setup(obj => obj.GetAll())
                .Returns(_locationsQueryableMock.Object);
            _mapperMock.Setup(obj => obj.Map<LocationDto>(_location))
                .Returns(_locationDto);

            var locationResult = await _locationService.GetById(_location.Id);

            locationResult.Should().Be(_locationDto);
        }

        [Test]
        public async Task GetAll_NoParametersPassed_ReturnsListOfLocationDtos()
        {
            _locationRepositoryMock.Setup(s => s.GetAll()).Returns(_locationsQueryableMock.Object);
            _mapperMock.Setup(obj => obj.Map<List<LocationDto>>(
                    It.Is<List<Location>>(x => ListsHasSameElements(x, _locations))))
                .Returns(_locationsDto);

            var locationResult = await _locationService.GetAll();

            locationResult.Should().BeEquivalentTo(_locationsDto);
        }

        [Test]
        public async Task GetAll_PageableParamsPassed_ReturnsPaginatedLocations()
        {
            var pageableParams = new FullPaginationQueryParams();
            var paginatedLocations = new PaginationDto<LocationDto>
            {
                Page = _locationsDto,
                TotalCount = _locationsDto.Count
            };
            _locationRepositoryMock.Setup(obj => obj.GetAll())
                .Returns(_locationsQueryableMock.Object);
            _paginationServiceMock.Setup(obj => obj.GetPageAsync<LocationDto, Location>(
                    _locationsQueryableMock.Object, 
                    pageableParams))
                .ReturnsAsync(paginatedLocations);

            var locationResult = await _locationService.GetAll(pageableParams);

            locationResult.Should().Be(paginatedLocations);
        }

        [Test]
        public async Task RemoveLocation_LocationExists_ReturnsLocationDtoRemoved()
        {
            _locationRepositoryMock.Setup(s => s.FindByIdAsync(_location.Id))
                .ReturnsAsync(_location);
            _mapperMock.Setup(obj => obj.Map<LocationDto>(_location))
                .Returns(_locationDto);

            var locationResult = await _locationService.Remove(_location.Id);

            _locationRepositoryMock.Verify(obj => obj.Remove(_location), Times.Once);
            _locationRepositoryMock.Verify(obj => obj.SaveChangesAsync(), Times.Once);
            
            locationResult.Should().Be(_locationDto);
        }

        [Test]
        public async Task RemoveLocation_LocationNotExist_ReturnsNull()
        {
            _locationRepositoryMock.Setup(s => s.FindByIdAsync(_location.Id))
                .ReturnsAsync(value: null);

            var locationResult = await _locationService.Remove(_location.Id);

            locationResult.Should().BeNull();
        }

        [Test]
        public async Task Update_ShouldUpdateLocationInDatabase()
        {
            _mapperMock.Setup(obj => obj.Map<Location>(_locationDto))
                .Returns(_location);

            await _locationService.Update(_locationDto);

            _locationRepositoryMock.Verify(obj => obj.Update(_location), Times.Once);
            _locationRepositoryMock.Verify(obj => obj.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Add_ShouldAddLocationToDatabase()
        {
            _mapperMock.Setup(obj => obj.Map<Location>(_locationDto))
                .Returns(_location);

            await _locationService.Add(_locationDto);

            _locationRepositoryMock.Verify(obj => obj.Add(_location), Times.Once);
            _locationRepositoryMock.Verify(obj => obj.SaveChangesAsync(), Times.Once);
        }

        private void MockData()
        {
            _locations = new List<Location>
            {
                new Location()
                {
                    Id = 1,
                    City = "Lviv",
                    IsActive = true,
                    Street = "Panasa Myrnogo",
                    OfficeName = "HQ",
                    UserRoom = new List<UserRoom> { new UserRoom { Id = 1, LocationId = 1, RoomNumber = "33" } }
                },
                new Location()
                {
                    Id = 2,
                    City = "Lviv",
                    IsActive = true,
                    Street = "Gorodotska",
                    OfficeName = "1",
                    UserRoom = new List<UserRoom> { new UserRoom { Id = 2, LocationId = 2, RoomNumber = "1" } }
                }
            };

            _locationsDto = _locations.Select(location => new LocationDto
            {
                Id = location.Id,
                City = location.City,
                IsActive = location.IsActive,
                OfficeName = location.OfficeName,
                Street = location.Street,
                Rooms = location.UserRoom.Select(userRoom => userRoom.RoomNumber).ToList()
            }).ToList();

            _locationsQueryableMock = _locations.AsQueryable().BuildMock();
            _location = _locations.FirstOrDefault();
            _locationDto = _locationsDto.FirstOrDefault();
        }

        private bool ListsHasSameElements(List<Location> obj1, List<Location> obj2)
        {
            var tempList1 = obj1.Except(obj2).ToList();
            var tempList2 = obj2.Except(obj1).ToList();

            return !(tempList1.Any() || tempList2.Any());
        }
    }
}
