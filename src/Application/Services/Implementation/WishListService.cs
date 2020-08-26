using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
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
        private readonly IEmailSenderService _emailSenderService;
        private readonly INotificationsService _notificationsService;

        public WishListService(
            IUserResolverService userResolverService,
            IPaginationService paginationService,
            IEmailSenderService emailSenderService,
            INotificationsService notificationsService,
            IRepository<Wish> wishRepository,
            IRepository<Book> bookRepository)
        {
            _userResolverService = userResolverService;
            _wishRepository = wishRepository;
            _paginationService = paginationService;
            _bookRepository = bookRepository;
            _notificationsService = notificationsService;
            _emailSenderService = emailSenderService;
        }

        public async Task<PaginationDto<BookGetDto>> GetWishesOfCurrentUserAsync(PageableParams pageableParams)
        {
            var currentUserId = _userResolverService.GetUserId();

            var wishesQuery = _wishRepository.GetAll()
                .Include(wish => wish.Book.Language)
                .Include(wish => wish.Book.BookAuthor).ThenInclude(bookAuthor => bookAuthor.Author)
                .Include(wish => wish.Book.BookGenre).ThenInclude(bookGenre => bookGenre.Genre)
                .Include(wish => wish.Book.User.UserRoom.Location)
                .Where(wish => wish.UserId == currentUserId)
                .Select(wish => wish.Book);
            var wishesPaginated =
                await _paginationService.GetPageAsync<BookGetDto, Book>(wishesQuery, pageableParams);

            return wishesPaginated;
        }

        public async Task AddWishAsync(int bookId)
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

        public async Task RemoveWishAsync(int bookId)
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

        public async Task NotifyAboutAvailableBookAsync(int bookId)
        {
            var wishes = await _wishRepository.GetAll()
                .Where(wish => wish.BookId == bookId)
                .Include(wish => wish.User)
                .Include(wish => wish.Book).ToListAsync();

            foreach (var wish in wishes)
            {
                await _notificationsService.NotifyAsync(
                    wish.User.Id, 
                    $"The book '{wish.Book.Name}' from your wish list is available now.", 
                    wish.BookId, 
                    NotificationAction.Request);
                if (wish.User.IsEmailAllowed)
                {
                    await _emailSenderService.SendForWishBecameAvailable(
                        $"{wish.User.FirstName} {wish.User.LastName}".Trim(),
                        wish.BookId,
                        wish.Book.Name,
                        wish.User.Email);
                }
            }
        }

        public async Task<bool> CheckIfBookInWishListAsync(int bookId)
        {
            var currentUserId = _userResolverService.GetUserId();

            var wish = await _wishRepository.FindByIdAsync(currentUserId, bookId);

            return wish != null;
        }

        public async Task<int> GetNumberOfWishedBooksAsync(int userId)
        {
            return await _wishRepository.GetAll().Where(wish => wish.UserId == userId).CountAsync();
        }
    }
}
