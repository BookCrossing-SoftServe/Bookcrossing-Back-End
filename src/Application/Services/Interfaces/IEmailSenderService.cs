using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.Email;
using Application.Dto.Password;

namespace Application.Services.Interfaces
{
    public interface IEmailSenderService
    {
        /// <summary>
        /// Sending email to notify that book was delivered to user
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForGotBookAsync(RequestMessage message);

        /// <summary>
        /// Sending email to notify that request was canceled
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForCanceledRequestAsync(RequestMessage message);

        /// <summary>
        /// Sending email to user that his book was requested
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForRequestAsync(RequestMessage message);

        /// <summary>
        /// Sending email to user that want to reset his password
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="confirmNumber">Unique confirmation number that is available only for 30 minutes</param>
        /// <param name="email">User email</param>
        /// <returns></returns>
        Task SendForPasswordResetAsync(string userName, string confirmNumber, string email);
    }
}
