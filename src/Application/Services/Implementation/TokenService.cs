using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public TokenService(IConfiguration configuration, IMapper mapper, IRepository<User> userRepository, IRepository<RefreshToken> refreshTokenRepository)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<RefreshToken> VerifyRefreshToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetAll().Include(p => p.User).FirstOrDefaultAsync(p => p.Token.Equals(token));
            if (refreshToken == null) throw new SecurityTokenException("Invalid refresh token");
            return refreshToken;
        }


        public async Task<User> VerifyUserCredentials(LoginDto loginModel)
        {
            var user = await _userRepository.GetAll()
                .Include(r => r.Role)
                .FirstOrDefaultAsync(p => p.Email == loginModel.Email);
            
            if(user == null)
            {
                throw new InvalidCredentialException("User not found");
            }

            if(!String.IsNullOrWhiteSpace(loginModel.AzureId) &&
                _passwordHasher.VerifyHashedPassword(user, user.AzureId, loginModel.AzureId) == PasswordVerificationResult.Success &&
                !user.IsDeleted)
            {
                return user;
            }

            if (_passwordHasher.VerifyHashedPassword(user, user.Password, loginModel.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialException("Password doesn't fit");
            }
                

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

        public async Task<string> GenerateRefreshToken(User user)
        {
            var randomNumber = new byte[32];
            string refreshToken;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
                RefreshToken refreshTokenModel = new RefreshToken();
                refreshTokenModel.UserId = user.Id;
                refreshTokenModel.Token = refreshToken;
                _refreshTokenRepository.Add(refreshTokenModel);
            }

            await _refreshTokenRepository.SaveChangesAsync();
            return refreshToken;
        }

        public string GenerateJWT(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        public async Task<string> UpdateRefreshRecord(RefreshToken refreshToken)
        {
            var randomNumber = new byte[32];
            string refresh;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refresh = Convert.ToBase64String(randomNumber);
                refreshToken.Token = refresh;
                _refreshTokenRepository.Update(refreshToken);
            }
            await _refreshTokenRepository.SaveChangesAsync();
            return refresh;
        }
    }
}
