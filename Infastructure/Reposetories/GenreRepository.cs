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
        public void AddNewGenre(Genre genre)
        {
            _context.Genre.Add(genre);
            _context.SaveChanges();
        }

        public IEnumerable<Genre> GetAllGenres() => _context.Genre.ToList();

        public void RemoveGenreById(int genreId)
        {
            var genre = _context.Genre.FirstOrDefault(p => p.Id == genreId);
            _context.Genre.Remove(genre);
            _context.SaveChanges();
        }
    }
}
