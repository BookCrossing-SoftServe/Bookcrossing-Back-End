using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        public List<string> FieldMasks { get; set; }

    }
}
