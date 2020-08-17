using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.SignalR;

namespace Application.SignalR.UserIdProviders
{
    public class UserIdProvider: IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return (connection.User?.Identity as ClaimsIdentity)?.FindFirst("id")?.Value;
        }
    }
}
