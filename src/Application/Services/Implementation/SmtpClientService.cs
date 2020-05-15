using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.Email;
using MailKit.Net.Smtp;
using MimeKit;
using ISmtpClient = Application.Services.Interfaces.ISmtpClient;

namespace Application.Services.Implementation
{
    public class SmtpClientService : ISmtpClient
    {
        public async Task SendAsync(MimeMessage mailMessage, EmailConfiguration emailConfiguration)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(emailConfiguration.UserName, emailConfiguration.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
