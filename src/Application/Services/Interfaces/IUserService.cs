using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> VerifyUserCredentials(LoginDto loginModel);


    }
}
