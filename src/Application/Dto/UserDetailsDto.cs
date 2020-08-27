using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    class UserDetailsDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsEmailAllowed { get; set; }

        public DateTime BirthDate { get; set; }

        public RoomLocationDto UserLocation { get; set; }

        public virtual List<BookDetailsDto> Books { get; set; }
    }
}
