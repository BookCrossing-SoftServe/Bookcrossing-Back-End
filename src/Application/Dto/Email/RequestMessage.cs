using System;
using Domain.RDBMS.Entities;
using MimeKit;

namespace Application.Dto.Email
{
    public class RequestMessage
    {
        public string OwnerName { get; set; }
        public MailboxAddress OwnerAddress { get; set; }
        public string UserName { get; set; }
        public MailboxAddress UserAddress { get; set; }
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string BookName { get; set; }
        public int BookId { get; set; }
    }
}
