using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IUserLocationRepository
    {
        UserLocation GetLocationByUser(User user);
    }
}
