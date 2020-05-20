using System.Collections.Generic;

namespace Domain.RDBMS.Entities
{
    public class Author : IEntityBase 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsConfirmed { get; set; }

        public virtual List<BookAuthor> BookAuthor { get; set; } 
    }
}
