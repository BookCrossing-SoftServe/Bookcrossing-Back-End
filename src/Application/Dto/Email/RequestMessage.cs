using System;
using Domain.RDBMS.Entities;
using MimeKit;

namespace Application.Dto.Email
{
    public class RequestMessage
    {
        public string UserName { get; set; }
        public MailboxAddress UserEmail { get; set; }
        public string RequestedUser { get; set; }
        public int RequestNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string Subject { get; set; }
        public string BookName { get; set; }
        public int BookId { get; set; }
    }
}
