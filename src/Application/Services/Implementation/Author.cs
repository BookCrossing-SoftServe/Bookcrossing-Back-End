using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Domain.IRepositories;
using Domain.Entities;

namespace Application.Services.Implementation
{
    public class Author : Interfaces.IAuthor
    {
        private readonly IAuthorRepository _authorRepository;
        public Author(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task Add(Domain.Entities.Author author)
        {
            await _authorRepository.AddAsync(author);
            await _authorRepository.SaveChangesAsync();
        }

        public async Task<Domain.Entities.Author> GetById(int authorId)
        {
            var author = await _authorRepository.FindByIdAsync(authorId);
            await _authorRepository.SaveChangesAsync();
            return author;
        }

        public async Task Remove(Domain.Entities.Author author)
        {
            _authorRepository.Remove(author);
            await _authorRepository.SaveChangesAsync();
        }

        public async Task Update(Domain.Entities.Author author)
        {
            _authorRepository.Update(author);
            await _authorRepository.SaveChangesAsync();
        }
    }
}
