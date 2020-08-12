using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class NotificationsService : INotificationsService
    {
        private readonly IRepository<Notification> _notificationsRepository;
        private readonly IUserResolverService _userResolverService;
        private readonly IMapper _mapper;

        public NotificationsService(IRepository<Notification> notificationsRepository, IUserResolverService userResolverService, IMapper mapper)
        {
            _notificationsRepository = notificationsRepository;
            _userResolverService = userResolverService;
            _mapper = mapper;
        }

        public async Task NotifyAsync(User user, string message)
        {
            var newNotification = new Notification{
                UserId = user.Id,
                Message = message
            };

            _notificationsRepository.Add(newNotification);
            await _notificationsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationDto>> GetAllForCurrentUserAsync()
        {
            var currentUserId = _userResolverService.GetUserId();

            var notifications = _notificationsRepository.GetAll()
                .Where(notification => notification.UserId == currentUserId);

            return _mapper.Map<IEnumerable<NotificationDto>>(await notifications.ToListAsync());
        }

        public async Task MarkAsReadAsync(int id)
        {
            var currentUserId = _userResolverService.GetUserId();
            var notification = await _notificationsRepository.FindByIdAsync(id);
            if (notification == null)
            {
                throw new ObjectNotFoundException($"Notification with id = {id} was not found");
            }

            if (notification.UserId != currentUserId)
            {
                throw new InvalidOperationException($"You cannot mark as read notifications that are not yours");
            }

            notification.IsRead = true;
            _notificationsRepository.Update(notification);
            await _notificationsRepository.SaveChangesAsync();
        }

        public async Task MarkAllAsReadForCurrentUserAsync()
        {
            var currentUserId = _userResolverService.GetUserId();
            var notifications =  _notificationsRepository.GetAll()
                .Where(notification => notification.UserId == currentUserId);
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _notificationsRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var currentUserId = _userResolverService.GetUserId();
            var notification = await _notificationsRepository.FindByIdAsync(id);
            if (notification == null)
            {
                throw new ObjectNotFoundException($"Notification with id = {id} was not found");
            }

            if (notification.UserId != currentUserId)
            {
                throw new InvalidOperationException($"You cannot mark as read notifications that are not yours");
            }

            _notificationsRepository.Remove(notification);
            await _notificationsRepository.SaveChangesAsync();
        }

        public async Task RemoveAllForCurrentUserAsync()
        {
            var currentUserId = _userResolverService.GetUserId();
            var notifications = _notificationsRepository.GetAll()
                .Where(notification => notification.UserId == currentUserId);

            _notificationsRepository.RemoveRange(notifications);
            await _notificationsRepository.SaveChangesAsync();
        }
    }
}