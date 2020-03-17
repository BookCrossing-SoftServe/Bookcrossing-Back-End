using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    class RequestRepository:BaseRepository<Request, int>, IRequestRepository
    {
        public RequestRepository(DbContext context) : base(context)
        {

        }
    }
}
