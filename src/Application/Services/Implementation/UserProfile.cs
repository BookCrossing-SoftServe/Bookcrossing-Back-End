using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;

namespace Application.Services.Implementation
{
    public class UserProfile : IUserProfile
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserLocationRepository _locationRepository;
        public UserProfile(IUserRepository userRepository, IBookRepository bookRepository, IUserLocationRepository locationRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _locationRepository = locationRepository;
        }

        public void AddNewBook(Book book)
        {
            _bookRepository.AddAsync(book);
        }

        public UserProfileDto GetMyProfile(int userId)
        {
            var user = _userRepository.FindByIdAsync(userId);
            var userProfile = new UserProfileDto();
            try
            {
                //Got to add these to repository first
                //userProfile.FirstName = user.FirstName;
                //userProfile.LastName = user.LastName;
                //userProfile.MiddleName = user.MiddleName;
                //userProfile.Email = user.Email;
                //userProfile.AllUserBooks = _bookRepository.GetBookByUser(user);
                //userProfile.UserLocation = _locationRepository.GetLocationByUser(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return userProfile;
        }
    }
}
