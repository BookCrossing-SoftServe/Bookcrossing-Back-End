using Application.Dto;
using Domain.Entities;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.IRepositories;
using Infrastructure.Repositories;
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
        public async Task<UserDto> Validate(LoginDto loginModel)
        {

            var user = _mapper.Map<UserDto>(_userRepository
                .GetAll()
                .Where(p => p.Email == loginModel.Email && p.Password == loginModel.Password)
                .Include(r => r.Role)
                .FirstOrDefault());

            return user;

        }
    }
}
