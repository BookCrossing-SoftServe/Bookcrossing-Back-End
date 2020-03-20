using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IAuthor
    {
        Task<Author> GetById(int authorId);
        Task<List<Author>> GetAll();
        Task Update(AuthorDto author);
        Task Remove(Author author);
        Task<Author> Add(AuthorDto author);
    }
}
