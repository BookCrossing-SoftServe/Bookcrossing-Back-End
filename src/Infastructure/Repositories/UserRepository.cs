using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepositories;
using Infastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BookCrossingContext context) : base(context)
        {
            
        }

        

    }
}
