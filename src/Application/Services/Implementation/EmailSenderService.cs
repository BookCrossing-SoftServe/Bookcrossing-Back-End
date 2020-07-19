using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.Email;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using ISmtpClient = Application.Services.Interfaces.ISmtpClient;

namespace Application.Services.Implementation
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IHostingEnvironment _env;
        private readonly ISmtpClient _smtpClient;
        private readonly string UnsubscribeURL = "https://book-crossing-dev.herokuapp.com/email/?email=";

        public EmailSenderService(EmailConfiguration emailConfig, IHostingEnvironment env, ISmtpClient smtpClient)
        {
            _emailConfig = emailConfig;
            _env = env;
            _smtpClient = smtpClient;
        }
        /// <inheritdoc />
        public async Task SendReceiveConfirmationAsync(string userName, string bookName, int bookId, int requestId, string userAddress)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "RequestReceiveConfirmation.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{USER.NAME}", userName);
            body = body.Replace("{BOOK.NAME}", bookName);
            body = body.Replace("{BOOK.ID}", bookId.ToString());
            body = body.Replace("{UnsubscribeURL}", UnsubscribeURL + userAddress + "&number=" + CreateSecurityHash(userAddress));

            var message = new Message(new List<string>() { userAddress },
                "Book crossing book receive confirmation!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }
        /// <inheritdoc />
        public async Task SendThatBookWasReceivedAsync(RequestMessage requestMessage)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "RequestReceived.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{OWNER.NAME}", requestMessage.OwnerName);
            body = body.Replace("{REQUEST.ID}", Convert.ToString(requestMessage.RequestId));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", UnsubscribeURL + requestMessage.OwnerAddress + "&number=" + CreateSecurityHash(requestMessage.OwnerAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.OwnerAddress.ToString() },
                $"Your book {requestMessage.BookName} was received!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }
        /// <inheritdoc />
        public async Task SendForCanceledRequestAsync(RequestMessage requestMessage)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "RequestCanceled.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{OWNER.NAME}", requestMessage.OwnerName);
            body = body.Replace("{USER.NAME}", requestMessage.UserName);
            body = body.Replace("{REQUEST.ID}", Convert.ToString(requestMessage.RequestId));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", UnsubscribeURL + requestMessage.OwnerAddress + "&number=" + CreateSecurityHash(requestMessage.OwnerAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.OwnerAddress.ToString() },
                $"Request for {requestMessage.BookName} was canceled!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }
        /// <inheritdoc />
        public async Task SendForBookDeactivatedAsync(RequestMessage requestMessage)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "BookDeactivated.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{USER.NAME}", requestMessage.UserName);
            body = body.Replace("{BOOK.ID}", Convert.ToString(requestMessage.BookId));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", UnsubscribeURL + requestMessage.UserAddress + "&number=" + CreateSecurityHash(requestMessage.UserAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.UserAddress.ToString() },
                $"Book {requestMessage.BookName} was deactivated!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }
        /// <inheritdoc />
        public async Task SendForBookActivatedAsync(RequestMessage requestMessage)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "BookActivated.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{USER.NAME}", requestMessage.UserName);
            body = body.Replace("{BOOK.ID}", Convert.ToString(requestMessage.BookId));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", UnsubscribeURL + requestMessage.UserAddress + "&number=" + CreateSecurityHash(requestMessage.UserAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.UserAddress.ToString() },
                $"Book {requestMessage.BookName} was activated!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }

        /// <inheritdoc />
        public async Task SendForRequestAsync(RequestMessage requestMessage)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "RequestEmail.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{OWNER.NAME}", requestMessage.OwnerName);
            body = body.Replace("{USER.NAME}", requestMessage.UserName);
            body = body.Replace("{REQUEST.ID}", Convert.ToString(requestMessage.RequestId));
            body = body.Replace("{REQUEST.DATE}", requestMessage.RequestDate.ToString("MMMM dd, yyyy"));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", UnsubscribeURL + requestMessage.OwnerAddress + "&number=" + CreateSecurityHash(requestMessage.OwnerAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.OwnerAddress.ToString() },
                $"Request for {requestMessage.BookName}!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }

        /// <inheritdoc />
        public async Task SendForPasswordResetAsync(string userName, string confirmNumber, string email)
        {
            string body = string.Empty;
            using (StreamReader reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, "Templates", "ResetPassword.html")))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{USER.NAME}", userName);
            body = body.Replace("{CONFIRM.NUMBER}", confirmNumber);
            body = body.Replace("{EMAIL}", email);
            body = body.Replace("{UnsubscribeURL}", UnsubscribeURL + email + "&number=" + CreateSecurityHash(email));

            var message = new Message(new List<string>() { email },
                "Book crossing password reset!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
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

        private string CreateSecurityHash(string email)
        {
            return string.Join(null, SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(email)).Select(x => x.ToString("x2")));
        }
    }
}
