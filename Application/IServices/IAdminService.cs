using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.IServices
{
    public interface IAdminService
    {
        IEnumerable<User> ListAllUsers();
        void AddNewGenre(Genre genre);
        void RemoveGenre(int genreId);
        IEnumerable<Genre> ListAllGenres();
        void RemoveUser(int userId);
    }
}
