using System.Collections.Generic;

namespace Domain.RDBMS.Entities
{
    public class UserRoom : IEntityBase
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int RoomNumber { get; set; }

        public virtual Location Location { get; set; }
        public virtual List<User> User { get; set; }
    }
}
