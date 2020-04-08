using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        public AuthorService(IRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDto> GetById(int authorId)
        {
            return _mapper.Map<AuthorDto>(await _authorRepository.FindByIdAsync(authorId));
        }

        public async Task<PaginationDto<AuthorDto>> GetAuthors(QueryParameters query)
        {
            var wrapper = new PaginationDto<AuthorDto>();
            var result = _authorRepository.GetAll();
            if (query.SearchQuery != null)
            {
                result = result.Where(a=> a.LastName.Contains((query.SearchQuery)));
            }
            if (query.FirstRequest)
            {
                wrapper.TotalCount = await result.CountAsync();
            }
            wrapper.Page = _mapper.Map<List<AuthorDto>>(await GetPage(result, query.Page, query.PageSize));
            return wrapper;
        }
        public async Task<AuthorDto> Add(NewAuthorDto newAuthorDto)
        {
            var author = _mapper.Map<Author>(newAuthorDto);
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
            await _authorRepository.SaveChangesAsync();
            return true;
        }
        public async Task Update(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _authorRepository.Update(author);
            await _authorRepository.SaveChangesAsync();
        }

        private async Task<List<Author>> GetPage(IQueryable<Author> query, int page, int pageSize)
        {
            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
