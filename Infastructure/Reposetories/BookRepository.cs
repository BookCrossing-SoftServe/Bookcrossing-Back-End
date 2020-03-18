using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(DbContext context) : base(context)
        {

        }
    }
}
