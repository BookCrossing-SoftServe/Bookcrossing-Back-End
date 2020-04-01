using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class RequestDto
    {
        public int Id { get; set; }
        public BookDto Book { get; set; }
        public UserDto Owner { get; set; }
        public UserDto User { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
    }
}
