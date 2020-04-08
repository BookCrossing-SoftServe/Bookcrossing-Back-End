using Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ILocationService
    {
        /// <summary>
        /// Retrieve location by ID
        /// </summary>
        /// <param name="locationId">Location's ID</param>
        /// <returns>returns Book DTO</returns>
        Task<LocationDto> GetById(int locationId);

        /// <summary>
        /// Retrieve all locations
        /// </summary>
        /// <returns>returns list of Location DTOs</returns>
        Task<List<LocationDto>> GetAll();

        /// <summary>
        /// Update specified location
        /// </summary>
        /// <param name="location">Location DTO instance</param>
        /// <returns></returns>
        Task Update(LocationDto location);

        /// <summary>
        /// Remove location from database
        /// </summary>
        /// <param name="locationId">Location's ID</param>
        /// <returns>Returns removed Location DTO</returns>
        Task<LocationDto> Remove(int locationId);

        /// <summary>
        /// Create new location and add it into Database
        /// </summary>
        /// <param name="location">Location DTO instance</param>
        /// <returns>Returns inserted Location's ID</returns>
        Task<int> Add(LocationDto location);
    }
}
