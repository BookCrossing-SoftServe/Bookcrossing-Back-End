using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface IAphorismService
    {
        /// <summary>
        /// Change current aphorism
        /// </summary>
        /// <returns>Return Task</returns>
        Task MoveToNextAsync();

        /// <summary>
        /// Get current aphorism
        /// </summary>
        /// <returns>Returns Aphorism DTO</returns>
        Task<AphorismDto> GetAphorismAsync();
    }
}
