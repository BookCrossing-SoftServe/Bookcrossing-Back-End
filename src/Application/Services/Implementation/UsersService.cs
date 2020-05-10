using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IRepository<UserLocation> _userLocationRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IRepository<ResetPassword> _resetPasswordRepository;

        public UsersService(IRepository<User> userRepository,IMapper mapper, IEmailSenderService emailSenderService, IRepository<ResetPassword> resetPasswordRepository, IRepository<UserLocation> userLocationRepository)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            _emailSenderService = emailSenderService;
            _resetPasswordRepository = resetPasswordRepository;
            _userLocationRepository = userLocationRepository;
        }
        ///<inheritdoc/>
        public async Task<UserDto> GetById(Expression<Func<User, bool>> predicate)
        {

            var user = await _userRepository.GetAll()
                .Include(i => i.UserLocation).ThenInclude(i => i.Location)
                .Include(x=>x.Role)
                .FirstOrDefaultAsync(predicate);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }
        public async Task<List<UserDto>> GetAllUsers()
        {
            return _mapper.Map<List<UserDto>>(await _userRepository.GetAll().Include(p => p.UserLocation).ToListAsync());
        }

        public async Task UpdateUser(UserUpdateDto userUpdateDto)
        {
            var user = _mapper.Map<User>(userUpdateDto);
            await _userRepository.Update(user, userUpdateDto.FieldMasks);
            var affectedRows = await _userRepository.SaveChangesAsync();
            if (affectedRows==0)
            {
                throw new DbUpdateException();
            }
        }

        public async Task RemoveUser(int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            _userRepository.Remove(user);
            var afftectedRows = await _userRepository.SaveChangesAsync();
            if (afftectedRows==0)
            {
                throw new DbUpdateException();
            }
        }
        /// <inheritdoc />
        public async Task SendPasswordResetConfirmation(string email)
        {
            var user = await _userRepository.FindByCondition(c => c.Email == email);
            var resetPassword = new ResetPassword
            {
                ConfirmationNumber = Guid.NewGuid().ToString(),
                ResetDate = DateTime.UtcNow
            };
            _resetPasswordRepository.Add(resetPassword);
            await _resetPasswordRepository.SaveChangesAsync();
             await _emailSenderService.SendForPasswordResetAsync(user.FirstName, resetPassword.ConfirmationNumber, email);

        }
        /// <inheritdoc />
        public async Task ResetPassword(ResetPasswordDto newPassword)
        {
            const int EXPIRATION_TIME = 30;
            var user = await _userRepository.FindByCondition(u => u.Email == newPassword.Email);
            var resetPassword =
                _resetPasswordRepository.FindByCondition(c => c.ConfirmationNumber == newPassword.ConfirmationNumber).Result;
            if (resetPassword != null && resetPassword.ConfirmationNumber == newPassword.ConfirmationNumber && resetPassword.ResetDate <= DateTime.Now.AddMinutes(EXPIRATION_TIME))
            {
                user.Password = newPassword.Password;
                await _userRepository.SaveChangesAsync();
            }
            await _userRepository.SaveChangesAsync();
        }
    }
}
