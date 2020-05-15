using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.Email;
using Application.Services.Interfaces;
using MimeKit;

namespace Application.Services.Implementation
{
    public class FakeSmtpClientService : ISmtpClient
    {
        public bool MailSent { get; set; }
        public FakeSmtpClientService()
        {
            MailSent = false;
        }
        public async Task SendAsync(MimeMessage mailMessage, EmailConfiguration emailConfiguration)
        {
            MailSent = true;
        }
    }
}
