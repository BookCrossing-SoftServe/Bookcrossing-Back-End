using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {

        }
        public virtual bool IsValidUser(User login)
        { 
            var loginResult= _context.Set<User>()
                .FirstOrDefault(u => u.Email == login.Email & u.Password == login.Password);
            return loginResult == null;
        }
    }
}
