using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Password;
using Domain.RDBMS.Entities;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
       
        Task<List<UserDto>> GetAllUsers();

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns></returns>
        Task<UserDto> GetById(Expression<Func<User, bool>> predicate);

        Task UpdateUser(UserUpdateDto userDto);

        Task<RegisterDto> AddUser(RegisterDto userRegisterDto);

        Task RemoveUser(int userId);

        /// <summary>
        /// Sending an email with unique confirmation code for password reset
        /// </summary>
        /// <param name="email">The email the letter will be sent to</param>
        /// <returns></returns>
        Task SendPasswordResetConfirmation(string email);

        /// <summary>
        /// Setting a new password
        /// </summary>
        /// <param name="newPassword">New password</param>
        /// <returns></returns>
        Task ResetPassword(ResetPasswordDto newPassword);

        Task ForbidEmailNotification(ForbidEmailDto email);
    }
}
