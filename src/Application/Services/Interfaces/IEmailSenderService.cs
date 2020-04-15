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
        Task SendEmailAsync(Message message);
        Task SendEmailForRequestAsync(RequestMessage message);
        Task SendEmailForPasswordResetAsync(string userName, string confirmNumber, string email);
    }
}
