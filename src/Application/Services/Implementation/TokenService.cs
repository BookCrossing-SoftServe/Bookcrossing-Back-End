using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.RDBMS.Entities;

namespace Application.Services.Implementation
{
    public class TokenService : ITokenService
    {
        IConfiguration configuration { get; set; }

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public string GenerateJSONWebToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id",user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,user.Role.Name)
            };
            var token = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"], audience: configuration["Jwt:Issuer"], claims,
                expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }
    }
}
