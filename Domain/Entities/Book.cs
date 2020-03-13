using System.Collections.Generic;

namespace Domain.Entities
{
    public class Book : Domain.Entities.IEntityBase
    { 

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Publisher { get; set; }
        public bool Available { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<BookAuthor> BookAuthor { get; set; } = new HashSet<BookAuthor>();
        public virtual ICollection<BookGenre> BookGenre { get; set; } = new HashSet<BookGenre>();
        public virtual ICollection<Request> Request { get; set; } = new HashSet<Request>();
    }
}
