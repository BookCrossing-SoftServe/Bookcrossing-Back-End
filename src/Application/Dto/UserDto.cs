using Domain.RDBMS.Entities;
using System.Collections.Generic;

namespace Application.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Role Role { get; set; }

        public List<RoomLocationDto> UserLocation { get; set; }
    }
}
