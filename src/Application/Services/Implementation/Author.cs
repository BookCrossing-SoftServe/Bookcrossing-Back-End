using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Domain.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class Author : Interfaces.IAuthor
    {
        private readonly IAuthorRepository _authorRepository;
        public Author(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Domain.Entities.Author> GetById(int authorId)
        {
            return await _authorRepository.FindByIdAsync(authorId);
        }

        public async Task<List<Domain.Entities.Author>> GetAll()
        {
            return await _authorRepository.GetAllAsync();
        }
        public async Task<Domain.Entities.Author> Add(AuthorDto authorDto)
        {
            var author = new Domain.Entities.Author() { FirstName = authorDto.FirstName, LastName = authorDto.LastName, MiddleName = authorDto.MiddleName };
            _authorRepository.Add(author);
            await _authorRepository.SaveChangesAsync();
            return author;
        }
        public async Task Remove(Domain.Entities.Author author)
        {
            _authorRepository.Remove(author);
            await _authorRepository.SaveChangesAsync();
        }
        public async Task Update(AuthorDto authorDto)
        {
            var author = new Domain.Entities.Author() { FirstName = authorDto.FirstName, LastName = authorDto.LastName, MiddleName = authorDto.MiddleName, Id = authorDto.Id };
            _authorRepository.Update(author);
            await _authorRepository.SaveChangesAsync();
        }
    }
}
