using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.QueryableExtension;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Infrastructure.RDBMS;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly BookCrossingContext _context;

        public AuthorService(
            IRepository<Author> authorRepository, 
            IMapper mapper, 
            IPaginationService paginationService,
            IRepository<BookAuthor> bookAuthorRepository, 
            BookCrossingContext context)
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

        public async Task<List<AuthorDto>> GetAll(int[] ids)
        {
            var predicate = PredicateBuilder.New<Author>();
            foreach (var id in ids)
            {
                var tempId = id;
                predicate = predicate.Or(a => a.Id == tempId);
            }
            var authors = _authorRepository.GetAll().Where(predicate);
            return _mapper.Map<List<AuthorDto>>(await authors.ToListAsync());
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
                                                                     || x.LastName.StartsWith(filter))
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

        public async Task<AuthorDto> Merge(AuthorMergeDto authorMergeDto)
        {
            await using var transaction = _context.Database.BeginTransaction();

            authorMergeDto.Author.IsConfirmed = true;
            authorMergeDto.Author.Id = null;
            var author = _mapper.Map<Author>(authorMergeDto.Author);

            var bookIds = await _bookAuthorRepository.GetAll().Where(x => authorMergeDto.Authors.Contains(x.AuthorId)).Select(x => x.BookId).Distinct().ToListAsync();

            var authors = await _authorRepository.GetAll().Where(x => authorMergeDto.Authors.Contains(x.Id)).ToListAsync();
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
            return affectedRows > 0 ? _mapper.Map<AuthorDto>(author) : null;

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