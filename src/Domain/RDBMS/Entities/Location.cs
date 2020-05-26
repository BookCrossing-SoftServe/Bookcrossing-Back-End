using System.Collections.Generic;

namespace Domain.RDBMS.Entities
{
    public class Location : IEntityBase
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string OfficeName { get; set; }
        public bool IsActive { get; set; }
        public virtual List<UserRoom> UserRoom { get; set; } 
    }
}
