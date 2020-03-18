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
    class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(DbContext context) : base(context)
        {

        }

        public void AddNewGenre(Genre genre)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Genre> GetAllGenres() => _context.Genre.ToList();

        public void RemoveGenreById(int genreId)
        {
            throw new NotImplementedException();
        }
    }
}
