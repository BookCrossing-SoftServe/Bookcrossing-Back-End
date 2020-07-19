using System.Collections.Generic;

namespace Application.Dto
{
    public class UserProfileDto
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<RoomLocationDto> UserLocation { get; set; }

        public bool IsEmailAllowed { get; set; }

        public virtual List<BookPutDto> Books { get; set; } 

    }
}
