using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Entities;
namespace Application.IServices
{
    public interface IUserService
    {
        Task<User> Validate(LoginModel loginModel);


    }
}
