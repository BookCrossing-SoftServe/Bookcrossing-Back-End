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

        public UsersService(IRepository<User> userRepository,IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }
        public async Task<UserDto> VerifyUserCredentials(LoginDto loginModel)
        {
            var user = _mapper.Map<UserDto>(await _userRepository.GetAll()
                .Include(r => r.Role)
                .FirstOrDefaultAsync(p => p.Email == loginModel.Email && p.Password == loginModel.Password));
            
            if(user==null) throw new InvalidCredentialException();

            return user;
        }
    }
}
