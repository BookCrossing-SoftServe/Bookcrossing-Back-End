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
