using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Reposetories
{
    public class BookAuthorRepository:BaseRepository<BookAuthor>,IBookAuthorRepository
    {
        public BookAuthorRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
