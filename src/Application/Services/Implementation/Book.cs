using Application.Dto;
using AutoMapper;
using Domain.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class Book : Interfaces.IBook
    {
        private readonly IRepository<Entities.Book> _bookRepository;
        private readonly IMapper _mapper;
        public Book(IRepository<Entities.Book> bookRepository, IMapper mapper)
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
            var book = _mapper.Map<Entities.Book>(bookDto);
            _bookRepository.Add(book);
            await _bookRepository.SaveChangesAsync();
            return book.Id;
        }

        public async Task<BookDto> Remove(int bookId)
        {
            var book = await _bookRepository.FindByIdAsync(bookId);
            if (book == null)
                return null;
            _bookRepository.Remove(book);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }

        public async Task Update(BookDto bookDto)
        {
            var book = _mapper.Map<Entities.Book>(bookDto);
            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();
        }
    }
}
