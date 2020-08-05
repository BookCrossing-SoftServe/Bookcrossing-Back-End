using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface IAphorismService
    {
        /// <summary>
        /// Retrieve aphorism by ID
        /// </summary>
        /// <param name="aphorismId">Language ID</param>
        /// <returns>returns Aphorism DTO</returns>
        Task<AphorismDto> GetById(int aphorismId);
    }
}
