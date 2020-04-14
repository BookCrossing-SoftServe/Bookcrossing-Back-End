using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Implementation
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }
        public int GetUserId()
        {
            var claimsIdentity = _context.HttpContext.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst("id")?.Value;
            return int.Parse(userId);
        }

        public bool IsUserAdmin()
        {
            return _context.HttpContext.User.IsInRole("Admin");
        }
    }
}
