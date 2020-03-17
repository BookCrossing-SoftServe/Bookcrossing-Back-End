using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    class BookAuthorRepository:BaseRepository<BookAuthor>,IBookAuthorRepository
    {
        public BookAuthorRepository(DbContext context) : base(context)
        {

        }
    }
}
