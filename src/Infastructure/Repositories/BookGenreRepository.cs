using Domain.Entities;
using Domain.IRepositories;
using Infastructure;

namespace Infrastructure.Repositories
{
    public class BookGenreRepository:BaseRepository<BookGenre>, IBookGenreRepository
    {
        public BookGenreRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
