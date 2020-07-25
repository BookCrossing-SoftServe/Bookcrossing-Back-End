using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;

namespace Application.Services.Interfaces
{
    public interface IWishListService
    {
        Task<PaginationDto<BookGetDto>> GetWishesOfCurrentUserAsync(PageableParams pageableParams);

        Task AddWishAsync(int bookId);

        Task RemoveWishAsync(int bookId);

        Task NotifyAboutAvailableBookAsync(int bookId);

        Task<bool> CheckIfBookInWishListAsync(int bookId);
    }
}
