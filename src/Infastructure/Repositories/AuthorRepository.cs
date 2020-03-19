using Domain.Entities;
using Domain.IRepositories;
using Infastructure;

namespace Infrastructure.Repositories
{
    public class AuthorRepository:BaseRepository<Author>,IAuthorRepository
    {
        public AuthorRepository(BookCrossingContext context) : base(context)
        {
        }
    }
}
