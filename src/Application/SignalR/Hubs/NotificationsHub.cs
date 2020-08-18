using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.SignalR.Hubs
{
    [Authorize]
    public class NotificationsHub : Hub
    {
        public const string URL = "/notifications/hub";
    }
}
