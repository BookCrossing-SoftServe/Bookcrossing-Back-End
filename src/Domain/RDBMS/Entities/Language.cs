using System.Collections.Generic;

namespace Domain.RDBMS.Entities
{
    public class Language : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Book> Books { get; set; }
    }
}
