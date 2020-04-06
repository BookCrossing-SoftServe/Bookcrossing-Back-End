using System.Collections.Generic;

namespace Domain.RDBMS.Entities
{
    public class User : IEntityBase
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual List<Book> Book { get; set; } 
        public virtual List<Request> RequestOwner { get; set; } 
        public virtual List<Request> RequestUser { get; set; }
        public virtual List<UserLocation> UserLocation { get; set; }
    }
}
