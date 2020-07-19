using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class WishListService : IWishListService
    {
        private readonly IUserResolverService _userResolverService;
        private readonly IRepository<Wish> _wishRepository;
        private readonly IPaginationService _paginationService;

        public WishListService(IUserResolverService userResolverService, IRepository<Wish> wishRepository, IPaginationService paginationService)
        {
            _userResolverService = userResolverService;
            _wishRepository = wishRepository;
            _paginationService = paginationService;
        }

        public async Task<PaginationDto<BookGetDto>> GetWishesOfCurrentUser(PageableParams pageableParams)
        {
            var currentUserId = _userResolverService.GetUserId();

            var wishesQuery = _wishRepository.GetAll()
                .Where(wish => wish.UserId == currentUserId)
                .Select(wish => wish.Book);
            var wishesPaginated = await _paginationService.GetPageAsync<BookGetDto, Book>(wishesQuery, pageableParams);

            return wishesPaginated;
        }

        public async void AddWish(int bookId)
        {
            var currentUserId = _userResolverService.GetUserId();

            var newWish = new Wish
            {
                UserId = currentUserId,
                BookId = bookId
            };

            _wishRepository.Add(newWish);
            await _wishRepository.SaveChangesAsync();
        }

        public async void RemoveWish(int bookId)
        {
            var currentUserId = _userResolverService.GetUserId();

            var wishForRemoving = await _wishRepository.FindByIdAsync(currentUserId, bookId);

            _wishRepository.Remove(wishForRemoving);
            await _wishRepository.SaveChangesAsync();
        }
    }
}
