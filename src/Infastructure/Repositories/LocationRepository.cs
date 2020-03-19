using Domain.Entities;
using Domain.IRepositories;
using Infastructure;

namespace Infrastructure.Repositories
{
    public class LocationRepository:BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
