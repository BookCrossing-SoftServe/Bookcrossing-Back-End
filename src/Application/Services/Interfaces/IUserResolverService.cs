using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Interfaces
{
    public interface IUserResolverService
    {
        public int GetUserId();
        public bool IsUserAdmin();
    }
}
