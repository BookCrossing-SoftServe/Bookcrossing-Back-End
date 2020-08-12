using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;

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
