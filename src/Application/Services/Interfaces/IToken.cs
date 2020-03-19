using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Interfaces
{
    public interface IToken
    {
        public string GenerateJSONWebToken(User user);
    }
}
