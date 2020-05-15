using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.Email;
using MimeKit;

namespace Application.Services.Interfaces
{
    public interface ISmtpClient
    {
        Task SendAsync(MimeMessage mailMessage, EmailConfiguration emailConfiguration);
    }
}
