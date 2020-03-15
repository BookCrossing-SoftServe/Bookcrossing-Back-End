using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.IServices
{
    public interface IAdminService
    {
        /// <summary>
        /// List of all users in admin panel
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> ListAllUsers();
        /// <summary>
        /// Function for adding new genre
        /// </summary>
        /// <param name="genre">Gerne object</param>
        void AddNewGenre(Genre genre);
        /// <summary>
        /// Function for removing genre by Id
        /// </summary>
        /// <param name="genreId">Genre Id</param>
        void RemoveGenre(int genreId);
        /// <summary>
        /// List of all genres in admin panel
        /// </summary>
        /// <returns></returns>
        IEnumerable<Genre> ListAllGenres();
        /// <summary>
        /// Remove user by user's id
        /// </summary>
        /// <param name="userId">User id</param>
        void RemoveUser(int userId);
    }
}
