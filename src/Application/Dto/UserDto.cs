using System;
using Domain.RDBMS.Entities;

namespace Application.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredDate { get; set; }

        public Role Role { get; set; }

        public bool IsEmailAllowed { get; set; }

        public RoomLocationDto UserLocation { get; set; }

        public int? numberOfBooksOwned { get; set; }
    }
}
