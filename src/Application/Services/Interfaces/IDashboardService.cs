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
        /// <summary>
        /// Retrieves all aggregated data needed for dashboard
        /// </summary>
        /// <param name="city">data will be aggregated only for this city, optional</param>
        /// <returns></returns>
        Task<DashboardDto> GetAll(string city = null);

        /// <summary>
        /// Retrieves location data
        /// </summary>
        /// <param name="city">data filtered based by city, optional</param>
        /// <returns></returns>
        Task<LocationDataDto> GetLocationData(string city = null);
        /// <summary>
        /// Retrieves users and books per day/month
        /// </summary>
        /// <param name="city">data will be aggregated only for this city, optional</param>
        /// <param name="byMonth">Whether to group by month(12) or day(30)</param>
        /// <returns></returns>
        Task<BookUserDataDto> GetBookUserData(string city = null, bool byMonth = true);

        /// <summary>
        /// Retrieves availability data
        /// </summary>
        /// <param name="city">data filtered based by city, optional</param>
        /// <returns></returns>
        Task<AvailabilityDataDto> GetAvailabilityData(string city = null);
    }
}
