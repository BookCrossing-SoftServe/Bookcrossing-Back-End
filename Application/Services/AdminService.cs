using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.IServices;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenreRepository _genreRepository;
        public AdminService(IGenreRepository genreRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _genreRepository = genreRepository;
        }

        public void AddNewGenre(Genre genre) => _genreRepository.AddNewGenre(genre);

        public IEnumerable<Genre> ListAllGenres() => _genreRepository.GetAllGenres();

        public IEnumerable<User> ListAllUsers() => _userRepository.GetAllUsers();

        public void RemoveGenre(int genreId) => _genreRepository.RemoveGenreById(genreId);

        public void RemoveUser(int userId) => _userRepository.RemoveUserById(userId);
    }
}
