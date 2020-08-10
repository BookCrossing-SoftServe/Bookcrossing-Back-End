using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
