using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    class BookGenreRepository:BaseRepository<BookGenre, int>, IBookGenreRepository
    {
        public BookGenreRepository(DbContext context) : base(context)
        {

        }
    }
}
