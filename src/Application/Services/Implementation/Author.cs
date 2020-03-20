using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        //THIS ONE SHOULD BE IN BOOK?
        public async Task<List<Domain.Entities.Author>> GetBooks(int authorId)
        {
            return await _authorRepository.GetAll().Where(a => a.Id == authorId).Include(a => a.BookAuthor).ThenInclude(b => b.Book).ToListAsync();
        }
        public async Task Add(Domain.Entities.Author author)
        {
            _authorRepository.Add(author);
            await _authorRepository.SaveChangesAsync();
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
