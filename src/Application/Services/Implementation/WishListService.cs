using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
        private readonly IRepository<Book> _bookRepository;
        private readonly IPaginationService _paginationService;

        public WishListService(IUserResolverService userResolverService, IPaginationService paginationService,
            IRepository<Wish> wishRepository, IRepository<Book> bookRepository)
        {
            _userResolverService = userResolverService;
            _wishRepository = wishRepository;
            _paginationService = paginationService;
            _bookRepository = bookRepository;
        }

        public async Task<PaginationDto<BookGetDto>> GetWishesOfCurrentUser(PageableParams pageableParams)
        {
            var currentUserId = _userResolverService.GetUserId();

            var wishesQuery = _wishRepository.GetAll()
                .Where(wish => wish.UserId == currentUserId)
                .Select(wish => wish.Book);
            var wishesPaginated = 
                await _paginationService.GetPageAsync<BookGetDto, Book>(wishesQuery, pageableParams);

            return wishesPaginated;
        }

        public async Task AddWish(int bookId)
        {
            var currentUserId = _userResolverService.GetUserId();
            var book = await _bookRepository.FindByIdAsync(bookId);

            if (book == null)
            {
                throw new ObjectNotFoundException($"There is no book with id = {bookId} in database");
            }

            if (book.UserId == currentUserId)
            {
                throw new InvalidOperationException("User cannot add his book to wish list");
            }

            var newWish = new Wish
            {
                UserId = currentUserId,
                BookId = bookId
            };

            _wishRepository.Add(newWish);
            await _wishRepository.SaveChangesAsync();
        }

        public async Task RemoveWish(int bookId)
        {
            var currentUserId = _userResolverService.GetUserId();

            var wishForRemoving = await _wishRepository.FindByIdAsync(currentUserId, bookId);
            if (wishForRemoving == null)
            {
                throw new InvalidOperationException($"Cannot delete book with id = {bookId} from current user's wish list, because there is no book with id = {bookId}");
            }

            _wishRepository.Remove(wishForRemoving);
            await _wishRepository.SaveChangesAsync();
        }
    }
}
