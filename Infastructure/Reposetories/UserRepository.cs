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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
