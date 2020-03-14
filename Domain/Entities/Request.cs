using System;

namespace Domain.Entities
{
    public class Request : IEntityBase
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string OwnerId { get; set; }
        public string UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ReceiveDate { get; set; }

        public virtual Book Book { get; set; }
        public virtual User Owner { get; set; }
        public virtual User User { get; set; }
    }
}
