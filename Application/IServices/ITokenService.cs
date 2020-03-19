using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.IServices
{
    public interface ITokenService
    {
        public string GenerateJSONWebToken(User user);
    }
}
