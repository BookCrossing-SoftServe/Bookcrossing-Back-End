using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using Infrastructure.RDBMS;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly BookCrossingContext _context;

        public AuthorService(IRepository<Author> authorRepository, IMapper mapper, IPaginationService paginationService,IRepository<BookAuthor> bookAuthorRepository, BookCrossingContext context)
        {
            _authorRepository = authorRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _context = context;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<AuthorDto> GetById(int authorId)
        {
            return _mapper.Map<AuthorDto>(await _authorRepository.FindByIdAsync(authorId));
        }

        public async Task<PaginationDto<AuthorDto>> GetAll(FullPaginationQueryParams parameters)
        {
            var query = _authorRepository.GetAll();
            return await _paginationService.GetPageAsync<AuthorDto, Author>(query, parameters);
        }

        public async Task<List<AuthorDto>> FilterAuthors(string filter)
        {
            return _mapper.Map<List<AuthorDto>>(await _authorRepository.GetAll()
                                                                 .Where(x => x.FirstName.StartsWith(filter) 
                                                                     || x.LastName.StartsWith(filter) 
                                                                     || x.MiddleName.StartsWith(filter))
                                                                 .ToListAsync());
        }

        public async Task<AuthorDto> Add(AuthorDto authorDto)
        {
            authorDto.Id = null;
            var author = _mapper.Map<Author>(authorDto);
            _authorRepository.Add(author);
            await _authorRepository.SaveChangesAsync();
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<bool> Remove(int authorId)
        {
            var author = await _authorRepository.FindByIdAsync(authorId);
            if (author == null)
            {
                return false;
            }

            _authorRepository.Remove(author);
            var affectedRows = await _authorRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> Merge(AuthorDto authorDto, int[] ids)
        {
            await using var transaction = _context.Database.BeginTransaction();

            authorDto.IsConfirmed = true;
            authorDto.Id = null;
            var author = _mapper.Map<Author>(authorDto);

            var bookIds = await _bookAuthorRepository.GetAll().Where(x => ids.Contains(x.AuthorId)).Select(x => x.BookId).Distinct().ToListAsync();

            var authors = await _authorRepository.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
            _authorRepository.RemoveRange(authors);
            await _authorRepository.SaveChangesAsync();

            var newBookAuthors = new List<BookAuthor>();
            foreach (var id in bookIds)
            {
                newBookAuthors.Add(new BookAuthor() { AuthorId = author.Id, BookId = id });
            }
            author.BookAuthor = newBookAuthors;

            _authorRepository.Add(author);

            var affectedRows = await _authorRepository.SaveChangesAsync();
            transaction.Commit();
            return affectedRows > 0;

        }
        public async Task<bool> Update(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _authorRepository.Update(author);
            var affectedRows = await _authorRepository.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}