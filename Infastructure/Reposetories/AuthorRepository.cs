using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    class AuthorRepository:BaseRepository<Author>,IAuthorRepository
    {
        public AuthorRepository(DbContext context) : base(context)
        {

        }
    }
}
