using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    class UpdatedUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int UserRoomId { get; set; }

        public List<string> FieldMasks { get; set; }
    }
}
