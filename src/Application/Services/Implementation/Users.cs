using Application.Dto;
using Domain.Entities;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Application.Services.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class Users : IUser
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public Users(IRepository<User> userRepository,IMapper mapper)
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
