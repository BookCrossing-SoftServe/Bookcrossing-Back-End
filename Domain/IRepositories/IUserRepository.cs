using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int userId);
        void AddNewUser(User user);
        void RemoveUserById(int userId);
    }
}
