using Domain.Entities;

namespace Domain.Entities
{
    public class BookAuthor : IEntityBase
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Book Book { get; set; }
    }
}
