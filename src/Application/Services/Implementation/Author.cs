using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Domain.IRepositories;
using Entities = Domain.Entities;

namespace Application.Services.Implementation
{
    public class Author : Interfaces.IAuthor
    {
        private readonly IAuthorRepository _authorRepository;
        public Author(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<AuthorDto> GetById(int authorId)
        {
            var author = await _authorRepository.FindByIdAsync(authorId);
            //mapper logic
            return new AuthorDto();
        }

        public async Task<List<AuthorDto>> GetAll()
        {
            var author = await _authorRepository.GetAllAsync();
            //mapper logic
            return new List<AuthorDto>();
        }
        public async Task<int> Add(AuthorDto authorDto)
        {
            var author = new Domain.Entities.Author() { FirstName = authorDto.FirstName, LastName = authorDto.LastName, MiddleName = authorDto.MiddleName };
            _authorRepository.Add(author);
            await _authorRepository.SaveChangesAsync();
            return author.Id;
        }
        public async Task<AuthorDto> Remove(int authorId)
        {
            var author = await _authorRepository.FindByIdAsync(authorId);
            if (author == null) 
                return null;
            _authorRepository.Remove(author);
            await _authorRepository.SaveChangesAsync();
            //mapper logic
            return new AuthorDto();
        }
        public async Task Update(AuthorDto authorDto)
        {
            var author = new Entities.Author() { FirstName = authorDto.FirstName, LastName = authorDto.LastName, MiddleName = authorDto.MiddleName, Id = authorDto.Id };
            _authorRepository.Update(author);
            await _authorRepository.SaveChangesAsync();
        }
    }
}
