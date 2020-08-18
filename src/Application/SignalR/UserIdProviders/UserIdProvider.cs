using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Application.SignalR.UserIdProviders
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return (connection.User?.Identity as ClaimsIdentity)?.FindFirst("id")?.Value;
        }
    }
}
