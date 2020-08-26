using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        private readonly IUserResolverService _userResolverService;
        private readonly IBookService _bookService;
        private readonly IRequestService _requestService;

        public StatisticsController(
            IWishListService wishListService, 
            IUserResolverService userResolverService,
            IRequestService requestService,
            IBookService bookService
        )
        {
            _wishListService = wishListService;
            _requestService = requestService;
            _userResolverService = userResolverService;
            _bookService = bookService;
        }

        [HttpGet("{counters}")]
        public async Task<ActionResult<CountersSetDto>> GetCounters()
        {
            var userId = _userResolverService.GetUserId();
            var wishedCount = await _wishListService.GetNumberOfWishedBooksAsync(userId);
            var requestedCount = await _requestService.GetNumberOfRequestedBooksAsync(userId);
            var readCount = await _bookService.GetNumberOfBooksInReadStatusAsync(userId);
            var numberOfTimesRegisteredBooksWereRead = await _bookService.GetNumberOfTimesRegisteredBooksWereReadAsync(userId);
            var countersDto = new CountersSetDto()
            {
                WishedBooksCount = wishedCount,
                RequestedBooksCount = requestedCount,
                ReadBooksCount = readCount,
                RegisteredBooksWereReadCount = numberOfTimesRegisteredBooksWereRead
            };

            return countersDto;
        }
    }
}
