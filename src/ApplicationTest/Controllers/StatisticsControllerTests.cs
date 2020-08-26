using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    internal class StatisticsControllerTests
    {
        private StatisticsController _controller;
        private Mock<IWishListService> _wishListServiceMock;
        private Mock<IUserResolverService> _userResolverServiceMock;
        private Mock<IRequestService> _requestServiceMock;
        private Mock<IBookService> _bookServiceMock;

        private CountersSetDto _countersSetDto;

        [OneTimeSetUp]
        public void InitializeClass()
        {
            _wishListServiceMock = new Mock<IWishListService>();
            _userResolverServiceMock = new Mock<IUserResolverService>();
            _requestServiceMock = new Mock<IRequestService>();
            _bookServiceMock = new Mock<IBookService>();
            _controller = new StatisticsController(
                _wishListServiceMock.Object,
                _userResolverServiceMock.Object,
                _requestServiceMock.Object,
                _bookServiceMock.Object
            );

            MockData();
        }

        [Test]
        public async Task GetCounters_ReturnsCountersSetDto()
        {
            _userResolverServiceMock.Setup(m => m.GetUserId())
                .Returns(It.IsAny<int>());
            _bookServiceMock.Setup(m => m.GetNumberOfBooksInReadStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(_countersSetDto.ReadBooksCount);
            _requestServiceMock.Setup(m => m.GetNumberOfRequestedBooksAsync(It.IsAny<int>()))
                .ReturnsAsync(_countersSetDto.RequestedBooksCount);
            _bookServiceMock.Setup(m => m.GetNumberOfTimesRegisteredBooksWereReadAsync(It.IsAny<int>()))
                .ReturnsAsync(_countersSetDto.RegisteredBooksWereReadCount);
            _wishListServiceMock.Setup(m => m.GetNumberOfWishedBooksAsync(It.IsAny<int>()))
                .ReturnsAsync(_countersSetDto.WishedBooksCount);

            var result = await _controller.GetCounters();

            result.Value.Should().BeEquivalentTo(_countersSetDto);
        }

        private void MockData()
        {
            _countersSetDto = new CountersSetDto()
            {
                RequestedBooksCount = 5,
                ReadBooksCount = 4,
                WishedBooksCount = 1,
                RegisteredBooksWereReadCount = 0
            };
        }
    }
}
