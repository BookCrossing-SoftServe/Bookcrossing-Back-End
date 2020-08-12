using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.RDBMS.Entities;

namespace Application.Services.Interfaces
{
    public interface INotificationsService
    {
        Task NotifyAsync(User user, string message);

        Task<IEnumerable<NotificationDto>> GetAllForCurrentUserAsync();

        Task MarkAsReadAsync(int id);

        Task MarkAllAsReadForCurrentUserAsync();

        Task RemoveAsync(int id);

        Task RemoveAllForCurrentUserAsync();
    }
}
