using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;

namespace Application.Services.Interfaces
{
    public interface IWishListService
    {
        Task<PaginationDto<BookGetDto>> GetWishesOfCurrentUser(PageableParams pageableParams);

        Task AddWish(int bookId);

        Task RemoveWish(int bookId);
    }
}
