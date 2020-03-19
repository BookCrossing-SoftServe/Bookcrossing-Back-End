using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.IRepositories;
using Infastructure;

namespace Infrastructure.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
