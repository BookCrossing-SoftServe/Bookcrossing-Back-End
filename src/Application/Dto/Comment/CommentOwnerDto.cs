using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Comment
{
    public class CommentOwnerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
