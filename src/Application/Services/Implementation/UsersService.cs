using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Password;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Infrastructure.RDBMS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class UsersService : IUserService
    {

        private readonly IRepository<User> _userRepository;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IRequestService _requestService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IRepository<ResetPassword> _resetPasswordRepository;
        private readonly IRepository<UserRoom> _userRoomRepository;
        private readonly IPaginationService _paginationService;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly BookCrossingContext _context; 

        public UsersService(IRepository<User> userRepository, IMapper mapper, IEmailSenderService emailSenderService, 
            IRepository<ResetPassword> resetPasswordRepository, IRepository<UserRoom> userRoomRepository, IBookService bookService, 
            BookCrossingContext context, IPaginationService paginationService, IRequestService requestService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
            _resetPasswordRepository = resetPasswordRepository;
            _userRoomRepository = userRoomRepository;
            _bookService = bookService;
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _paginationService = paginationService;
            _requestService = requestService;
        }
        ///<inheritdoc/>
        public async Task<UserDto> GetById(Expression<Func<User, bool>> predicate)
        {

            var user = await _userRepository.GetAll()
                .Include(i => i.UserRoom).ThenInclude(i => i.Location)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(predicate);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            return _mapper.Map<List<UserDto>>(await _userRepository.GetAll().Include(p => p.UserRoom).ToListAsync());
        }

        public async Task<PaginationDto<UserDto>> GetAllUsers(FullPaginationQueryParams parameters)
        {
            var userList = _userRepository.GetAll().Include(p => p.UserRoom);
            var paginatedListOfUsers = await _paginationService.GetPageAsync<UserDto, User>(userList, parameters);
            paginatedListOfUsers.Page.ForEach(async user => user.NumberOfBooksOwned = await _bookService.GetCurrentOwnedByIdCount(user.Id));
            return paginatedListOfUsers;
        }

        public async Task UpdateUser(UserUpdateDto userUpdateDto)
        {
            UserRoom newRoomId = _userRoomRepository.GetAll().FirstOrDefault(x => x.Location.Id == userUpdateDto.UserLocation.Location.Id
                                                && x.RoomNumber == userUpdateDto.UserLocation.RoomNumber);

            if (newRoomId == null)
            {
                newRoomId = new UserRoom() { LocationId = userUpdateDto.UserLocation.Location.Id, RoomNumber = userUpdateDto.UserLocation.RoomNumber };
                _userRoomRepository.Add(newRoomId);
                await _userRoomRepository.SaveChangesAsync();
            }

            var newUser = new UpdatedUserDto()
            {
                Id = userUpdateDto.Id,
                FirstName = userUpdateDto.FirstName,
                LastName = userUpdateDto.LastName,
                BirthDate = userUpdateDto.BirthDate,
                UserRoomId = newRoomId.Id,
                IsEmailAllowed = userUpdateDto.IsEmailAllowed,
                FieldMasks = userUpdateDto.FieldMasks
            };

            var user = _mapper.Map<User>(newUser);
            await _userRepository.Update(user, newUser.FieldMasks);
            var affectedRows = await _userRepository.SaveChangesAsync();
            if (affectedRows == 0)
            {
                throw new DbUpdateException();
            }
        }

        public async Task<RegisterDto> AddUser(RegisterDto userRegisterDto)
        {
            if (await _userRepository.FindByCondition(u => u.Email == userRegisterDto.Email) == null)
            {
                var user = _mapper.Map<User>(userRegisterDto);

                if(!String.IsNullOrEmpty(user.AzureId))
                {
                    user.AzureId = _passwordHasher.HashPassword(user, user.AzureId);
                }
                else
                {
                    user.Password = _passwordHasher.HashPassword(user, user.Password);
                }

                user.FirstName = Regex.Replace(user.FirstName, "[ ]+", " ");
                user.LastName = Regex.Replace(user.LastName, "[ ]+", " ");
                _userRepository.Add(user);
                await _userRepository.SaveChangesAsync();  
                return _mapper.Map<RegisterDto>(user);
            }
            else
                return null;
        }

        public async Task RemoveUser(int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var user = _userRepository.GetAll().Include(user => user.Book).Include(user => user.RequestUser).ThenInclude(requesr => requesr.Book).FirstOrDefault(user => user.Id == userId);
            if (user == null)
            {
                throw new ObjectNotFoundException($"There is no user with id = {userId} in database");
            }

            if (user.Book.Any(p => p.State != BookState.InActive))
            {
                throw new InvalidOperationException();
            }
            var requestsIds = user.RequestUser.Where(request => request.ReceiveDate == null).Select(request => request.Id).ToList();
            foreach(var requestId in requestsIds)
            {
                await _requestService.RemoveAsync(requestId);
            }
            user.IsDeleted = true;
            var affectedRows = await _userRepository.SaveChangesAsync();
            if (affectedRows == 0)
            {
                throw new DbUpdateException();
            }
            await transaction.CommitAsync();
        }

        public async Task RecoverDeletedUser(int userId)
        {
            var user = await _userRepository.FindByCondition(ar => ar.IsDeleted);
            if (user == null)
            {
                throw new ObjectNotFoundException($"There is no user with id = {userId} in database");
            }

            if (user.IsDeleted)
            {
                user.IsDeleted = false;
            }

            var affectedRows = await _userRepository.SaveChangesAsync();
            if (affectedRows == 0)
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
                user.Password = _passwordHasher.HashPassword(user, newPassword.Password);
                await _userRepository.SaveChangesAsync();
            }
            await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> ForbidEmailNotification(ForbidEmailDto email)
        {
            var user = await _userRepository.FindByCondition(u => u.Email == email.Email);
            if (user != null && string.Join(null, SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.Email)).Select(x => x.ToString("x2"))) == email.Code)
            {
                user.IsEmailAllowed = false;
                await _userRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
