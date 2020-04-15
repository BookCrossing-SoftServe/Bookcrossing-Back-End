using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Password;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
       
        Task<List<UserDto>> GetAllUsers();
        Task UpdateUser(UserUpdateDto userDto);

        Task RemoveUser(int userId);

   
        Task SendPasswordResetConfirmation(string email);
        Task ResetPassword(ResetPasswordDto newPassword);

    }
}
