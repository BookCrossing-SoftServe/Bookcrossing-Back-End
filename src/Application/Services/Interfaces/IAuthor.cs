using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IAuthor
    {
        Task<Author> GetById(int authorId);
        Task Update(Author author);
        Task Remove(Author author);
        Task Add(Author author);
    }
}
