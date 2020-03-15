using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    public class UserLocationRepository : IUserLocationRepository
    {
        private readonly BookCrossingContext _context;
        public UserLocationRepository(BookCrossingContext context)
        {
            _context = context;
        }

        public UserLocation GetLocationByUser(User user) =>
            _context.UserLocation.Include(u => u.Location).FirstOrDefault(i => i.UserId == user.Id);
    }
}
