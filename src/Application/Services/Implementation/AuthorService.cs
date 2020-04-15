using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IPaginationService _paginationService;
        public AuthorService(IRepository<Author> authorRepository, IMapper mapper, IPaginationService paginationService)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<AuthorDto> GetById(int authorId)
        {
            return _mapper.Map<AuthorDto>(await _authorRepository.FindByIdAsync(authorId));
        }

        public async Task<PaginationDto<AuthorDto>> GetAuthors(QueryParameters parameters)
        {
            var query = _authorRepository.GetAll();
            return await _paginationService.GetPage<AuthorDto, Author>(query, parameters);
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
        public async Task<bool> Update(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _authorRepository.Update(author);
            var affectedRows = await _authorRepository.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
