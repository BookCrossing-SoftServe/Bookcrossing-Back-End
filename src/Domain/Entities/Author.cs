using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Author : IEntityBase 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual List<BookAuthor> BookAuthor { get; set; } 
    }
}
