using Domain.Entities;
using Domain.IRepositories;
using Infastructure;

namespace Infrastructure.Repositories
{
    public class UserLocationRepository : BaseRepository<UserLocation>, IUserLocationRepository
    {
        public UserLocationRepository(BookCrossingContext context) : base(context)
        {

        }
    }
}
