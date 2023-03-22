using Applications.Services.EmailServices;
using Quartz;

namespace Application.Cronjob
{
    public class SendAttendanceMailJob : IJob
    {
        private readonly MailService _mailService;

        public SendAttendanceMailJob(MailService mailService)
        {
            _mailService = mailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _mailService.SendAbsentEmail();
        }
    }
}