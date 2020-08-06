using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.Email;

namespace Application.Services.Interfaces
{
    public interface IHangfireJobScheduleService
    {
        /// <summary>
        /// Sends email that asks user whether him get book or not in 9th day and make book available in 10th day after request 
        /// </summary>
        /// <param name="message">email mesage</param>
        Task ScheduleRequestJob(RequestMessage message);

        /// <summary>
        /// Delete schedule from db for ScheduleRequestJob() method
        /// </summary>
        /// <param name="requestId">requestId</param>
        Task DeleteRequestScheduleJob(int requestId);
    }
}
