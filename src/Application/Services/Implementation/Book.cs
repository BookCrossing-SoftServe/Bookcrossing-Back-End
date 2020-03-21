using Application.Dto;
using AutoMapper;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities = Domain.Entities;

namespace Application.Services.Implementation
{
    public class Book : Interfaces.IBook
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public Book(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> GetById(int bookId)
        {
            return _mapper.Map<BookDto>(await _bookRepository.FindByIdAsync(bookId));
        }

        public async Task<List<BookDto>> GetAll()
        {
            return _mapper.Map<List<BookDto>>(await _bookRepository.GetAllAsync());
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
