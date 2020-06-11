using System.Threading.Tasks;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Application.Dto.Dashboard;
using MongoDB.Driver;

namespace Application.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Location> _locationRepository;

        public DashboardService(IRepository<Book> bookRepository, IRepository<User> userRepository, IRepository<Location> locationRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _locationRepository = locationRepository;
        }

        public async Task<DashboardDto> GetAll(string city = null)
        {
            var result = new DashboardDto()
            {
                Cities = await _locationRepository.GetAll().GroupBy(x => x.City).Select(x => x.Key).ToListAsync(),
                AvailabilityData = await GetAvailabilityData(),
                BookUserComparisonData = await GetBookUserData(city),
                LocationData = await GetLocationData(city)
            };
            return result;        
        }
        private async Task<Dictionary<string, AvailabilityDataDto>> GetAvailabilityData()
        {
            var data = _bookRepository.GetAll().IgnoreQueryFilters();
            var dataResult = await data.GroupBy(x => new { x.State, x.User.UserRoom.Location.City })
                                        .Select(x => new { x.Key.City, x.Key.State, count = x.Count()}).ToListAsync();
            var dictionary = dataResult.GroupBy(x => x.City).ToDictionary(x => x.Key, x => x.ToDictionary( x=> x.State, x=> x.count));
            var cityData = new Dictionary<string, AvailabilityDataDto>();
            foreach (var city in dictionary)
            {
                cityData[city.Key] = new AvailabilityDataDto()
                {

                    Available = city.Value.ContainsKey(BookState.Available) ? city.Value[BookState.Available] : 0,
                    Requested = city.Value.ContainsKey(BookState.Requested) ? city.Value[BookState.Requested] : 0,
                    Reading = city.Value.ContainsKey(BookState.Reading) ? city.Value[BookState.Reading] : 0,
                    Deactivated = city.Value.ContainsKey(BookState.InActive) ? city.Value[BookState.InActive] : 0
                };
            }
            return cityData;
        }
        public async Task<AvailabilityDataDto> GetAvailabilityData(string city = null)
        {
            var data = _bookRepository.GetAll().IgnoreQueryFilters();
            if (city != null)
            {
                data = data.Where(x => x.User.UserRoom.Location.City == city);
            }
            var dataResult = await data.GroupBy(x => x.State).Select(x => new { State = x.Key, total = x.Count()}).ToDictionaryAsync(data => data.State, data => data.total);
            var result = new AvailabilityDataDto()
            {
                Available = dataResult.ContainsKey(BookState.Available) ? dataResult[BookState.Available] : 0,
                Requested = dataResult.ContainsKey(BookState.Requested) ? dataResult[BookState.Requested] : 0,
                Reading = dataResult.ContainsKey(BookState.Reading) ? dataResult[BookState.Reading] : 0,
                Deactivated = dataResult.ContainsKey(BookState.InActive) ? dataResult[BookState.InActive] : 0,
            };
            return result;
        }
        public async Task<BookUserDataDto> GetBookUserData(string city = null)
        {
            var data = _bookRepository.GetAll().IgnoreQueryFilters();
            var userData = _userRepository.GetAll().IgnoreQueryFilters();
            if (city != null)
            {
                data = data.Where(x => x.User.UserRoom.Location.City == city);
                userData = userData.Where(x => x.UserRoom.Location.City == city);
            };
            var result = new BookUserDataDto()
            {
                BooksRegistered = await data.GroupBy(x => x.DateAdded.Date).Select(x => new { x.Key, total = x.Count() }).ToDictionaryAsync(data => data.Key, data => data.total),
                UsersRegistered = await userData.GroupBy(x => x.RegisteredDate.Date).Select(x => new { x.Key, total = x.Count() }).ToDictionaryAsync(data => data.Key, data => data.total)
            };
            return result;
        }
        public async Task<LocationDataDto> GetLocationData(string city = null)
        {
            var data = _bookRepository.GetAll().IgnoreQueryFilters();
            var userData = _userRepository.GetAll().IgnoreQueryFilters();
            if (city != null)
            {
                data = data.Where(x => x.User.UserRoom.Location.City == city);
                userData = userData.Where(x => x.UserRoom.Location.City == city);
            }
            var result = new LocationDataDto()
            {
                TotalBooks = await data.CountAsync(),
                DeactivatedBooks = await data.Where(x => x.State == BookState.InActive).CountAsync(),
                TotalUsers = await userData.CountAsync()
            };
            return result;

        }
    }
}