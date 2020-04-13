using System;
using Application.Dto;
using System.Threading.Tasks;
using System.Security.Authentication;
using Application.Dto.Password;
using Application.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain.RDBMS;
using Domain.RDBMS.Entities;

namespace Application.Services.Implementation
{
    public class UsersService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;

        public UsersService(IRepository<User> userRepository,IMapper mapper, IEmailSenderService emailSenderService)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            _emailSenderService = emailSenderService;
        }
        public async Task<UserDto> VerifyUserCredentials(LoginDto loginModel)
        {
            var user = _mapper.Map<UserDto>(await _userRepository.GetAll()
                .Include(r => r.Role)
                .FirstOrDefaultAsync(p => p.Email == loginModel.Email && p.Password == loginModel.Password));
            
            if(user==null) throw new InvalidCredentialException();

            return user;
        }
        public async Task SendPasswordResetConfirmation(string email)
        {
            var user = await _userRepository.FindByCondition(c => c.Email == email);
            Random rand = new Random();
            var confirmation = 999;
            await _emailSenderService.SendEmailForPasswordResetAsync(user.FirstName, confirmation.ToString(), email);

        }
        public async Task ResetPassword(NewPasswordDto newPassword)
        {
            var user = await _userRepository.FindByCondition(u => u.Email == newPassword.Email);
            if (newPassword.ConfirmationNumber == 999)
            {
                user.Password = newPassword.Password;
                await _userRepository.SaveChangesAsync();
            }
        }
    }
}
