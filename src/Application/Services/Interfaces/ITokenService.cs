using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Dto;
using Domain.RDBMS.Entities;

namespace Application.Services.Interfaces
{
    public interface ITokenService
    {
        Task<User> VerifyUserCredentials(LoginDto loginModel);
        Task<RefreshToken> VerifyRefreshToken(string token);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        Task<string> GenerateRefreshToken(User user);

        Task<string> UpdateRefreshRecord(RefreshToken refreshToken);

        string GenerateJWT(IEnumerable<Claim> claims);
    }
}
