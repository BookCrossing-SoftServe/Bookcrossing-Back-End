using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateJSONWebToken(UserDto user);
        Task<UserDto> VerifyUserCredentials(LoginDto loginModel);
    }
}
