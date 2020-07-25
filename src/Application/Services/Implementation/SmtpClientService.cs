using System.Threading.Tasks;
using Application.Dto.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using ISmtpClient = Application.Services.Interfaces.ISmtpClient;

namespace Application.Services.Implementation
{
    public class SmtpClientService : ISmtpClient
    {
        private readonly ILogger<SmtpClientService> _logger;

        public SmtpClientService(ILogger<SmtpClientService> logger)
        {
            _logger = logger;
        }

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
                catch (SmtpCommandException ex)
                {
                    _logger.Log(LogLevel.Error, ex, ex.Message);
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
