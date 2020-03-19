using Application.IServices;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration configuration { get; set; }

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.FirstName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,"ggfhgjfgh")   
            };
            var token = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"], audience: configuration["Jwt:Issuer"], claims,
                expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }
    }
}
