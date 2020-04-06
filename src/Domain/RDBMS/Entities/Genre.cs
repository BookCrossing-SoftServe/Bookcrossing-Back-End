using System.Collections.Generic;

namespace Domain.RDBMS.Entities
{
    public class Genre : IEntityBase
    { 

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookGenre> BookGenre { get; set; } = new HashSet<BookGenre>();
    }
}
