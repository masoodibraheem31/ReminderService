using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reminder.BL.interfaces;
using Reminder.Entities.DTO;
using System;
using System.Threading.Tasks;

namespace ReminderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService reminderService;
        public ReminderController(IReminderService reminderService)
        {
            this.reminderService = reminderService;
        }

        [HttpPost, Route("save-reminder")]
        public async Task<IActionResult> SaveReminder(ReminderDTO reminder)
        {
            var response = await this.reminderService.SaveReminder(reminder);
            return Ok(response);
        }
        //[HttpPost, Route("start-reminder")]
        //public IActionResult StartReminder()
        //{
        //    var jobId = BackgroundJob.Schedule<IReminderService>(x => x.ScheduleReminder(), TimeSpan.FromSeconds(3));
        //    return Ok("Scheduled:" + jobId);
        //}
        [HttpPost, Route("schedule-reminder")]
        public IActionResult ScheduleReminder()
        {
            RecurringJob.AddOrUpdate(() => reminderService.ScheduleReminder(), Cron.Daily(), TimeZoneInfo.Local);
            return Ok("Success");
        }

    }
}