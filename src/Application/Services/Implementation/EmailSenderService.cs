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
using MimeKit.Cryptography;
using ISmtpClient = Application.Services.Interfaces.ISmtpClient;

namespace Application.Services.Implementation
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IWebHostEnvironment _env;
        private readonly ISmtpClient _smtpClient;
        private readonly string _unsubscribeUrl;
        private readonly string _templatesFolderName;

        public EmailSenderService(EmailConfiguration emailConfig, IWebHostEnvironment env, ISmtpClient smtpClient)
        {
            _emailConfig = emailConfig;
            _env = env;
            _smtpClient = smtpClient;
            _unsubscribeUrl = "https://book-crossing-dev.herokuapp.com/email/?email=";
            _templatesFolderName = "Templates";
        }
        /// <inheritdoc />
        public async Task SendReceiveConfirmationAsync(string userName, string bookName, int bookId, int requestId, string userAddress)
        {
            var body = await GetMessageTemplateFromFile("RequestReceiveConfirmation.html");

            body = body.Replace("{USER.NAME}", userName);
            body = body.Replace("{BOOK.NAME}", bookName);
            body = body.Replace("{BOOK.ID}", bookId.ToString());
            body = body.Replace("{UnsubscribeURL}", _unsubscribeUrl + userAddress + "&number=" + CreateSecurityHash(userAddress));

            var message = new Message(new List<string>() { userAddress },
                "Book crossing book receive confirmation!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }
        /// <inheritdoc />
        public async Task SendThatBookWasReceivedAsync(RequestMessage requestMessage)
        {
            var body = await GetMessageTemplateFromFile("RequestReceived.html");

            body = body.Replace("{OWNER.NAME}", requestMessage.OwnerName);
            body = body.Replace("{REQUEST.ID}", Convert.ToString(requestMessage.RequestId));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", _unsubscribeUrl + requestMessage.OwnerAddress + "&number=" + CreateSecurityHash(requestMessage.OwnerAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.OwnerAddress.ToString() },
                $"Your book {requestMessage.BookName} was received!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }
        /// <inheritdoc />
        public async Task SendForCanceledRequestAsync(RequestMessage requestMessage)
        {
            var body = await GetMessageTemplateFromFile("RequestCanceled.html");

            body = body.Replace("{OWNER.NAME}", requestMessage.OwnerName);
            body = body.Replace("{USER.NAME}", requestMessage.UserName);
            body = body.Replace("{REQUEST.ID}", Convert.ToString(requestMessage.RequestId));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", _unsubscribeUrl + requestMessage.OwnerAddress + "&number=" + CreateSecurityHash(requestMessage.OwnerAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.OwnerAddress.ToString() },
                $"Request for {requestMessage.BookName} was canceled!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }
        /// <inheritdoc />
        public async Task SendForBookDeactivatedAsync(RequestMessage requestMessage)
        {
            var body = await GetMessageTemplateFromFile("BookDeactivated.html");

            body = body.Replace("{USER.NAME}", requestMessage.UserName);
            body = body.Replace("{BOOK.ID}", Convert.ToString(requestMessage.BookId));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", _unsubscribeUrl + requestMessage.UserAddress + "&number=" + CreateSecurityHash(requestMessage.UserAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.UserAddress.ToString() },
                $"Book {requestMessage.BookName} was deactivated!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }
        /// <inheritdoc />
        public async Task SendForBookActivatedAsync(RequestMessage requestMessage)
        {
            var body = await GetMessageTemplateFromFile("BookDeactivated.html");

            body = body.Replace("{USER.NAME}", requestMessage.UserName);
            body = body.Replace("{BOOK.ID}", Convert.ToString(requestMessage.BookId));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", _unsubscribeUrl + requestMessage.UserAddress + "&number=" + CreateSecurityHash(requestMessage.UserAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.UserAddress.ToString() },
                $"Book {requestMessage.BookName} was activated!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }

        /// <inheritdoc />
        public async Task SendForRequestAsync(RequestMessage requestMessage)
        {
            var body = await GetMessageTemplateFromFile("RequestEmail.html");

            body = body.Replace("{OWNER.NAME}", requestMessage.OwnerName);
            body = body.Replace("{USER.NAME}", requestMessage.UserName);
            body = body.Replace("{REQUEST.ID}", Convert.ToString(requestMessage.RequestId));
            body = body.Replace("{REQUEST.DATE}", requestMessage.RequestDate.ToString("MMMM dd, yyyy"));
            body = body.Replace("{BOOK.NAME}", requestMessage.BookName);
            body = body.Replace("{UnsubscribeURL}", _unsubscribeUrl + requestMessage.OwnerAddress + "&number=" + CreateSecurityHash(requestMessage.OwnerAddress.ToString()));

            var message = new Message(new List<string>() { requestMessage.OwnerAddress.ToString() },
                $"Request for {requestMessage.BookName}!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }

        /// <inheritdoc />
        public async Task SendForPasswordResetAsync(string userName, string confirmNumber, string email)
        {
            var body = await GetMessageTemplateFromFile("ResetPassword.html");

            body = body.Replace("{USER.NAME}", userName);
            body = body.Replace("{CONFIRM.NUMBER}", confirmNumber);
            body = body.Replace("{EMAIL}", email);
            body = body.Replace("{UnsubscribeURL}", _unsubscribeUrl + email + "&number=" + CreateSecurityHash(email));

            var message = new Message(new List<string>() { email },
                "Book crossing password reset!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }

        public async Task SendForWishBecameAvailable(string userName, int bookId, string bookName, string email)
        {
            var body = await GetMessageTemplateFromFile("WishBecameAvailable.html");

            var bookUrl = $"https://book-crossing-dev.herokuapp.com/book/{bookId}";

            body = body.Replace("{USER.NAME}", userName);
            body = body.Replace("{BOOK.ID}", bookId.ToString());
            body = body.Replace("{BOOK.NAME}", bookName);
            body = body.Replace("{BOOK.URL}", bookUrl);
            body = body.Replace("{UnsubscribeURL}", _unsubscribeUrl + email + "&number=" + CreateSecurityHash(email));

            var message = new Message(new List<string>() { email },
                "Book from your wish list became available!", body);

            await _smtpClient.SendAsync(CreateEmailMessage(message), _emailConfig);
        }

        protected virtual async Task<string> GetMessageTemplateFromFile(string templateFileName)
        {
            using var reader =
                new StreamReader(Path.Combine(_env.ContentRootPath, _templatesFolderName, templateFileName));
            return await reader.ReadToEndAsync();
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
            return string.Join(
                null,
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(email)).Select(x => x.ToString("x2")));
        }
    }
}
