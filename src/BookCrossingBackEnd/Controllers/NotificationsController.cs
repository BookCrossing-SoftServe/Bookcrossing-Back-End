using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAll()
        {
            var notifications = await _notificationsService.GetAllForCurrentUserAsync();
            return Ok(notifications);
        }

        [HttpPut("read/{id}")]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            try
            {
                await _notificationsService.MarkAsReadAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpPut("read/all")]
        public async Task<ActionResult> MarkAllAsRead()
        {
            await _notificationsService.MarkAllAsReadForCurrentUserAsync();
            return Ok();
        }

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                await _notificationsService.RemoveAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpDelete("remove/all")]
        public async Task<ActionResult> RemoveAll()
        {
            await _notificationsService.RemoveAllForCurrentUserAsync();
            return Ok();
        }
    }
}
