using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
       
        Task<List<UserDto>> GetAllUsers();
        Task UpdateUser(UserUpdateDto userDto);

        Task RemoveUser(int userId);


    }
}
