using Domain.Entities;
using Domain.IRepositories;
using Infastructure;

namespace Infrastructure.Repositories
{
    public class RequestRepository:BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
