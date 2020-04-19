using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.Email;
using Application.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace Application.Services.Implementation
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IHostingEnvironment _env;

        public EmailSenderService(EmailConfiguration emailConfig, IHostingEnvironment env)
        {
            _emailConfig = emailConfig;
            _env = env;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

        public async Task SendEmailForRequestAsync(RequestMessage message)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "RequestEmail.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{USER}", message.UserName);
            body = body.Replace("{REQUEST.USER}", message.RequestedUser);
            body = body.Replace("{REQUEST.NUMBER}", Convert.ToString(message.RequestNumber));
            body = body.Replace("{REQUEST.DATE}", message.RequestDate.ToString("MMMM dd, yyyy"));
            body = body.Replace("{BOOK.NAME}", message.BookName);
            body = body.Replace("{BOOK.ID}", message.BookId.ToString());

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Book Crossing", _emailConfig.From));
            emailMessage.To.Add(message.UserEmail);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            await SendAsync(emailMessage);
        }
        public async Task SendEmailForPasswordResetAsync(string userName, string confirmNumber, string email)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "ResetPassword.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{USER}", userName);
            body = body.Replace("{CONFIRMNUMBER}", confirmNumber);
            body = body.Replace("{EMAIL}", email);

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Book Crossing", _emailConfig.From));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = "Book crossing password reset!";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Book Crossing",_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

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
