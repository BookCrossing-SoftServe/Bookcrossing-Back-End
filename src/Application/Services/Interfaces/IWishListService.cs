using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;

namespace Application.Services.Interfaces
{
    public interface IWishListService
    {
        Task<PaginationDto<BookGetDto>> GetWishesOfCurrentUser(PageableParams pageableParams);
        void AddWish(int bookId);
        void RemoveWish(int bookId);
    }
}
