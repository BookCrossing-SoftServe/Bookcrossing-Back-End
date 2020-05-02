using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto.Email;
using Application.Services.Interfaces;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Hangfire;
using MimeKit;

namespace Application.Services.Implementation
{
    public class HangfireJobSchedulerService : IHangfireJobScheduleService
    {
        private readonly IRepository<ScheduleJob> _scheduleRepository;
        public HangfireJobSchedulerService(IRepository<ScheduleJob> scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        public void ScheduleRequestJob(RequestMessage message)
        {
            var jobId = BackgroundJob.Schedule<EmailSenderService>(x => x.SendReceiveConfirmationAsync(message.UserName, message.BookName, 
                    message.BookId, message.RequestId, message.UserAddress.ToString()),
                TimeSpan.FromDays(9));
            _scheduleRepository.Add(new ScheduleJob{ ScheduleId = jobId, RequestId = message.RequestId});
            _scheduleRepository.SaveChangesAsync();
            var secondJobId = BackgroundJob.Schedule<RequestService>(x => x.RemoveAsync(message.RequestId),
                TimeSpan.FromDays(10));
            _scheduleRepository.Add(new ScheduleJob { ScheduleId = secondJobId, RequestId = message.RequestId });
            _scheduleRepository.SaveChangesAsync();
        }
        public void DeleteRequestScheduleJob(int requestId)
        {
            var records = _scheduleRepository.GetAll().Where(x => x.RequestId == requestId).ToList();
            foreach (var jobId in records)
            {
                BackgroundJob.Delete(jobId.ScheduleId);
            }
            _scheduleRepository.RemoveRange(records);
            _scheduleRepository.SaveChangesAsync();
        }
    }
}
