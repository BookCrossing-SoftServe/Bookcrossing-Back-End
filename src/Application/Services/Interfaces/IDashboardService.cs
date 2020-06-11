using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Dashboard;
using Application.Dto.QueryParams;
using Application.QueryableExtension;

namespace Application.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetAll(string city = null);
        Task<LocationDataDto> GetLocationData(string city = null);
        Task<BookUserDataDto> GetBookUserData(string city = null);
        Task<AvailabilityDataDto> GetAvailabilityData(string city = null);
    }
}
