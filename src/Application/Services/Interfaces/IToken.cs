using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface IToken
    {
        public string GenerateJSONWebToken(UserDto user);
    }
}
