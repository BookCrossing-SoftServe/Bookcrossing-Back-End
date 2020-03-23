using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class RequestDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int OwnerId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
    }
}
