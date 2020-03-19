using Domain.Entities;
using Domain.IRepositories;
using Infastructure;

namespace Infrastructure.Repositories
{
    public class BookAuthorRepository:BaseRepository<BookAuthor>,IBookAuthorRepository
    {
        public BookAuthorRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
