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

namespace Application.Services.Implementation
{
    public class NotificationsService : INotificationsService
    {
        private IRepository<Notification> _notificationsRepository;
        private IUserResolverService _userResolverService;
        private IMapper _mapper;

        public NotificationsService(IRepository<Notification> notificationsRepository, IUserResolverService userResolverService, IMapper mapper)
        {
            _notificationsRepository = notificationsRepository;
            _userResolverService = userResolverService;
            _mapper = mapper;
        }

        public IEnumerable<NotificationDto> GetNotificationsForCurrentUser()
        {
            var currentUserId = _userResolverService.GetUserId();

            var notifications = _notificationsRepository.GetAll()
                .Where(notification => notification.UserId == currentUserId);

            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task MarkNotificationAsReadAsync(int id)
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

        public async Task RemoveNotificationAsync(int id)
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
    }
}