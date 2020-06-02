using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Application.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;

        public TokenService(IConfiguration configuration,IMapper mapper, IRepository<User> userRepository)
        {
            this._configuration = configuration;
            this._mapper = mapper;
            this._userRepository = userRepository;
        }
        

        public async Task<TokenDto> GenerateTokens(User user,IEnumerable<Claim> claims)
        {
            var jwtToken = GenerateJWT(claims);

            var refreshToken = await GenerateRefreshToken(user);

            var tokenDto = new TokenDto
            {
                JWT = jwtToken,
                RefreshToken = refreshToken
            };

            return tokenDto;
        }

        public async Task<User> VerifyRefreshToken(string token)
        {
            var user = await _userRepository.FindByCondition(u => u.RefreshToken.Equals(token));
            if(user==null) throw new SecurityTokenException("Invalid refresh token");
            return user;
        }


        public async Task<User> VerifyUserCredentials(LoginDto loginModel)
        {
            var user = await _userRepository.GetAll()
                .Include(r => r.Role)
                .FirstOrDefaultAsync(p => p.Email == loginModel.Email && p.Password == loginModel.Password);

            if (user == null) throw new InvalidCredentialException();

            return user;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private async Task<string> GenerateRefreshToken(User user)
        {
            var randomNumber = new byte[32];
            string refreshToken;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
                user.RefreshToken = refreshToken;
                _userRepository.Update(user);
            }

            await _userRepository.SaveChangesAsync();
            return refreshToken;
        }

        private string GenerateJWT(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1.0), 
                signingCredentials: credentials);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

  

       
      




    }
}
