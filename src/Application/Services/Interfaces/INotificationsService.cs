using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface INotificationsService
    {
        IEnumerable<NotificationDto> GetNotificationsForCurrentUserAsync();

        Task MarkNotificationAsRead(int id);

        Task RemoveNotificationAsync(int id);
    }
}
