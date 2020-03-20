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
        /// <summary>
        /// Retrieve Author by ID
        /// </summary>
        /// <param name="authorId">Author's ID</param>
        /// <returns></returns>
        Task<Author> GetById(int authorId);

        /// <summary>
        /// Retrieve all Authors
        /// </summary>
        /// <returns></returns>
        Task<List<Author>> GetAll();

        /// <summary>
        /// Update specified Author
        /// </summary>
        /// <param name="author">Author's DTO instance</param>
        /// <returns></returns>
        Task Update(AuthorDto author);

        /// <summary>
        /// Remove specified Author
        /// </summary>
        /// <param name="author">Author's DTO instance</param>
        /// <returns></returns>
        Task Remove(Author author);

        /// <summary>
        /// Create new author and add it into DbContext
        /// </summary>
        /// <param name="author">Author's DTO instance</param>
        /// <returns></returns>
        Task<Author> Add(AuthorDto author);
    }
}
