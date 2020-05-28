using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Dto;
using Domain.RDBMS.Entities;

namespace Application.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokens(User user,IEnumerable<Claim> claims);
        Task<User> VerifyUserCredentials(LoginDto loginModel);
        Task<User> VerifyRefreshToken(string token);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
