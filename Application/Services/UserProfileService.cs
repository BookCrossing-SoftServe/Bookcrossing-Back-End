using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.IServices;
using Domain.Entities;
using Infrastructure;

namespace Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private BookCrossingContext _context;
        public UserProfileService(BookCrossingContext context)
        {
            _context = context;
        }
        public void GetMyProfile(Guid id)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when getting user by {nameof(id)}={id}: ", ex);
            }
        }
    }
}
