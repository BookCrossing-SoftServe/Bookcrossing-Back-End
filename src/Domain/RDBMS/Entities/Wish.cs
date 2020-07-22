namespace Domain.RDBMS.Entities
{
    public class Wish : IEntityBase
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}