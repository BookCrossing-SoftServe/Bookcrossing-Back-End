using Application.Dto;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.RDBMS.Entities;
using Domain.RDBMS;

namespace Application.Services.Implementation
{
    public class BookService : Interfaces.IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        public BookService(IRepository<Book> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
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

        public async Task<List<BookDto>> GetAll()
        {
            return _mapper.Map<List<BookDto>>(await _bookRepository.GetAll()
                                                                    .Include(p => p.BookAuthor)
                                                                    .ThenInclude(x => x.Author)
                                                                    .Include(p => p.BookGenre)
                                                                    .ThenInclude(x => x.Genre)
                                                                    .ToListAsync());
        }

        public async Task<int> Add(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _bookRepository.Add(book);
            await _bookRepository.SaveChangesAsync();
            return book.Id;
        }

        public async Task<BookDto> Remove(int bookId)
        {
            var book = await _bookRepository.GetAll()
                            .Include(p => p.BookAuthor)
                            .ThenInclude(x => x.Author)
                            .Include(p => p.BookGenre)
                            .ThenInclude(x => x.Genre)
                            .FirstOrDefaultAsync(p => p.Id == bookId);
            if (book == null)
                return null;
            _bookRepository.Remove(book);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }

        public async Task Update(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();
        }
    }
}
