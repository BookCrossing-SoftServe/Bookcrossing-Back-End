namespace Domain.RDBMS.Entities
{
    public class UserLocation : IEntityBase
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int RoomNumber { get; set; }

        public virtual Location Location { get; set; }
        public virtual User User { get; set; }
    }
}
