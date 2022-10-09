using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Entities.DTO
{
    public class ReminderDTO
    {
        public string Message { get; set; }
        public string Time { get; set; }
        public DateTime? EndDate { get; set; }
        public string TimeZone { get; set; }
        public DateTime? LastScheduledDateTime { get; set; }
        public string UserId { get; set; }
    }
}
