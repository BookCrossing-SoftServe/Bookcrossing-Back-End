using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.IServices;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services
{
    public class AdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenreRepository _genreRepository;
        public AdminService(IGenreRepository genreRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _genreRepository = genreRepository;
        }


        public void RemoveUser(User user) => _userRepository.Remove(user);
    }
}
