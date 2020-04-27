using Application.Dto;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.RDBMS.Entities;
using Domain.RDBMS;
using System.Linq;
using Application.Dto.QueryParams;
using Application.Dto.QueryParams.Enums;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using Infrastructure.RDBMS;

namespace Application.Services.Implementation
{
    public class BookService : Interfaces.IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        private readonly IRepository<UserLocation> _userLocationRepository;
        private readonly IPaginationService _paginationService;
        private readonly BookCrossingContext _context;
        private readonly IMapper _mapper;
        public BookService(IRepository<Book> bookRepository, IMapper mapper, IRepository<BookAuthor> bookAuthorRepository, IRepository<BookGenre> bookGenreRepository, IRepository<UserLocation> userLocationRepository, IPaginationService paginationService, BookCrossingContext context)
        {
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookGenreRepository = bookGenreRepository;
            _userLocationRepository = userLocationRepository;
            _paginationService = paginationService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookDto> GetById(int bookId)
        {
            return _mapper.Map<BookDto>(await _bookRepository.GetAll()
                                                               .Include(p => p.BookAuthor)
                                                               .ThenInclude(x => x.Author)
                                                               .Include(p => p.BookGenre)
                                                               .ThenInclude(x => x.Genre)
                                                               .FirstOrDefaultAsync(p => p.Id == bookId));
        }
        public async Task<PaginationDto<BookDto>> GetAll(BookQueryParams parameters)
        {
            //TODO: Create or replace BookDto to fit requirements.
            //TODO: Swagger doesn't support proper array queries, either do it by hand or use this.
            //TODO: Delete Test sample before merge.
            FilterParameters[] bookFilters =
            {
            };
            FilterParameters locationFilters = new FilterParameters() { PropertyName = "Location.Id", Value = "1", Method = FilterMethod.Equal };
            FilterParameters authorFilters = new FilterParameters() { PropertyName = "Author.LastName", Value = "Martin", Method = FilterMethod.Contains };
            FilterParameters[] genreFilters =
            {
                new FilterParameters() { PropertyName = "Genre.Name", Value = "Fantasy", Method = FilterMethod.Equal, Operand = FilterOperand.Or},
                new FilterParameters() { PropertyName = "Genre.Name", Value = "Horror", Method = FilterMethod.Equal}
            };

            var bookIds =
                from b in _bookRepository.GetAll().Where(bookFilters)
                join g in _bookGenreRepository.GetAll().Where(genreFilters) on b.Id equals g.BookId
                join a in _bookAuthorRepository.GetAll().Where(authorFilters) on b.Id equals a.BookId
                join l in _userLocationRepository.GetAll().Where(locationFilters) on b.UserId equals l.UserId
                select b.Id;

            var query = _bookRepository.GetAll().Where(x => bookIds.Contains(x.Id))
                .Include(p => p.BookAuthor)
                .ThenInclude(x => x.Author)
                .Include(p => p.BookGenre)
                .ThenInclude(x => x.Genre)
                .Include(p => p.User)
                .ThenInclude(x => x.UserLocation)
                .ThenInclude(x => x.Location);

            return await _paginationService.GetPageAsync<BookDto,Book>(query, parameters);
        }

        public async Task<BookDto> Add(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _bookRepository.Add(book);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> Remove(int bookId)
        {
            var book = await _bookRepository.GetAll()
                            .Include(p => p.BookAuthor)
                            .ThenInclude(x => x.Author)
                            .Include(p => p.BookGenre)
                            .ThenInclude(x => x.Genre)
                            .FirstOrDefaultAsync(p => p.Id == bookId);
            if (book == null)
                return false;
            _bookRepository.Remove(book);
            var affectedRows = await _bookRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> Update(BookDto bookDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var book = _mapper.Map<Book>(bookDto);
                var doesBookExist = await _bookRepository.GetAll().AnyAsync(a => a.Id == book.Id);
                if (!doesBookExist)
                {
                    return false;
                }
                _bookAuthorRepository.RemoveRange(await _bookAuthorRepository.GetAll().Where(a => a.BookId == book.Id).ToListAsync());
                _bookGenreRepository.RemoveRange(await _bookGenreRepository.GetAll().Where(a => a.BookId == book.Id).ToListAsync());
                await _bookRepository.SaveChangesAsync();
                _bookAuthorRepository.AddRange(book.BookAuthor);
                _bookGenreRepository.AddRange(book.BookGenre);
                _bookRepository.Update(book);
                var affectedRows = await _bookRepository.SaveChangesAsync();
                transaction.Commit();
                return affectedRows > 0;
            }
        }
    }
}
