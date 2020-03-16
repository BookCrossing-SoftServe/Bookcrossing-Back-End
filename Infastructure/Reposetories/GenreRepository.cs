using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;

namespace Infastructure.Reposetories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly BookCrossingContext _context;

        public GenreRepository(BookCrossingContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetAllGenres() => _context.Genre.ToList();

    }
}
