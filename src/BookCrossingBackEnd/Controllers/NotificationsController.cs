using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private NotificationsService _notificationsService;

        public NotificationsController(NotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<NotificationDto>> GetNotifications()
        {
            var notifications = _notificationsService.GetNotificationsForCurrentUser();
            return notifications.ToList();
        }

        [HttpPost("read")]
        public async Task<ActionResult> MarkAsRead([FromBody] int id)
        {
            try
            {
                await _notificationsService.MarkNotificationAsReadAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> RemoveNotification(int id)
        {
            try
            {
                await _notificationsService.RemoveNotificationAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }
    }
}
