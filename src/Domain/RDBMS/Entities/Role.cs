using System.Collections.Generic;

namespace Domain.RDBMS.Entities
{
    public class Role : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<User> User { get; set; }
    }
}
