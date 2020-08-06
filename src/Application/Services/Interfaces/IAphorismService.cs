using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface IAphorismService
    {
        /// <summary>
        /// Get next aphorism
        /// </summary>
        /// <returns>Returns Aphorism DTO</returns>
        Task<AphorismDto> GetNextAsync();
    }
}
