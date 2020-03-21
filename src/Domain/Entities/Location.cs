using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Location : IEntityBase
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string OfficeName { get; set; }

        //public virtual ICollection<UserLocation> UserLocation { get; set; } = new HashSet<UserLocation>();
        public virtual List<UserLocation> UserLocation { get; set; } 
    }
}
