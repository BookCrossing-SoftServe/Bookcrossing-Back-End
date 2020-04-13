using System.Collections.Generic;
using Application.Dto;
using System.Threading.Tasks;
using System.Security.Authentication;
using Application.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain.RDBMS;
using Domain.RDBMS.Entities;

namespace Application.Services.Implementation
{
    public class UsersService : IUserService
    {

        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UsersService(IRepository<User> userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }
      

        public async Task<List<UserDto>> GetAllUsers()
        {
            return _mapper.Map<List<UserDto>>(await _userRepository.GetAll().Include(p => p.UserLocation).ToListAsync());
        }

        public async Task UpdateUser(UserUpdateDto userUpdateDto)
        {
            var user = _mapper.Map<User>(userUpdateDto);
            _userRepository.Update(user);
            var updated = await _userRepository.SaveChangesAsync();
            if (!updated)
            {
                throw new DbUpdateException();
            }
        }

        public async Task RemoveUser(int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            _userRepository.Remove(user);
            var deleted = await _userRepository.SaveChangesAsync();
            if (!deleted)
            {
                throw new DbUpdateException();
            }
        }

    }
}
