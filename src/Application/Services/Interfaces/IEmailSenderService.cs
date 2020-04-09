using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(Message message);
    }
}
