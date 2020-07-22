using System;
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

        public string RefreshToken { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public int? UserRoomId { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredDate { get; set; }

        public bool IsEmailAllowed { get; set; }

        public virtual Role Role { get; set; }

        public virtual List<Book> Book { get; set; } 

        public virtual List<Request> RequestOwner { get; set; } 

        public virtual List<Request> RequestUser { get; set; }

        public virtual List<RefreshToken> RefreshTokens { get; set; }

        public virtual UserRoom UserRoom { get; set; }
        public virtual List<Wish> Wish { get; set; }
    }
}
