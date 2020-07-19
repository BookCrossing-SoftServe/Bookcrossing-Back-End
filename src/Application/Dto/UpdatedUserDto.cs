using System;
using System.Collections.Generic;

namespace Application.Dto
{
    class UpdatedUserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public int UserRoomId { get; set;}

        public bool IsEmailAllowed { get; set; }

        public List<string> FieldMasks { get; set; }
    }
}
