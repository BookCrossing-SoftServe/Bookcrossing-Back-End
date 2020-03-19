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
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class Users : IUser
    {
        BookCrossingContext dbCOntext;
        public Users(BookCrossingContext context)
        {
            this.dbCOntext = context;
        }
        public async Task<User> Validate(LoginDto loginModel)
        {
            var user = await dbCOntext.User.Include(p=>p.Role).AsQueryable().FirstOrDefaultAsync(user => user.Email == loginModel.Email && user.Password==loginModel.Password);
            
            return user;
        }
    }
}
