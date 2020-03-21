using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Role : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<User> User { get; set; }
    }
}
