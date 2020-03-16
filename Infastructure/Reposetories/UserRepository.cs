using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;

namespace Infastructure.Reposetories
{
    public class UserRepository : IUserRepository
    {
        public readonly BookCrossingContext _context;

        public UserRepository(BookCrossingContext context)
        {
            _context = context;
        }

        public void AddNewUser(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers() => _context.User.ToList();

        public User GetUserById(int userId) =>
            _context.User.FirstOrDefault(p => p.Id == userId);

        public void RemoveUserById(int userId) 
        {
            var user = _context.User.FirstOrDefault(p => p.Id == userId);
            _context.User.Remove(user);
            _context.SaveChanges();
        }
    }
}
