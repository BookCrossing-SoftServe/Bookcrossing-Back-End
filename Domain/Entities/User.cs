using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : IEntityBase
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Book> Book { get; set; } = new HashSet<Book>();
        public virtual ICollection<Request> RequestOwner { get; set; } = new HashSet<Request>();
        public virtual ICollection<Request> RequestUser { get; set; } = new HashSet<Request>();
        public virtual ICollection<UserLocation> UserLocation { get; set; } = new HashSet<UserLocation>();
    }
}
