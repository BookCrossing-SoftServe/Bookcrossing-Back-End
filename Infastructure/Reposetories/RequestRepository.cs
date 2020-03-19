using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    public class RequestRepository:BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
