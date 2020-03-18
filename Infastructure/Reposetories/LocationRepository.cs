using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    class LocationRepository:BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(DbContext context) : base(context)
        {

        }
    }
}
