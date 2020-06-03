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
        /// Sending email to confirm whether book was delivered
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="bookName"></param>
        /// <param name="bookId"></param>
        /// <param name="requestId"></param>
        /// <param name="userAddress"></param>
        /// <returns></returns>
        Task SendReceiveConfirmationAsync(string userName, string bookName, int bookId, int requestId, string userAddress);

        /// <summary>
        /// Sending email to notify that book was delivered to user
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendThatBookWasReceivedAsync(RequestMessage message);

        /// <summary>
        /// Sending email to notify that book was activated
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForBookActivatedAsync(RequestMessage message);

        /// <summary>
        /// Sending email to notify that book was deactivated
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForBookDeactivatedAsync(RequestMessage message);

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
