using Application.IServices;
using Application.Models;
using Domain.Entities;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UsersService : IUserService
    {
        BookCrossingContext dbCOntext;
        public UsersService(BookCrossingContext context)
        {
            this.dbCOntext = context;
        }
        public async Task<User> Validate(LoginModel loginModel)
        {
            var user = await dbCOntext.User.AsQueryable().FirstOrDefaultAsync(user => user.Email == loginModel.Email && user.Password==loginModel.Password);
            return user;
        }
    }
}
