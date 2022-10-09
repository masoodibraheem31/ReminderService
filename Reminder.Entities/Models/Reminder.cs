using System;
using System.Collections.Generic;

namespace Reminder.Entities.Models
{
    public class Reminder : BaseModel
    {
        public string Message { get; set; }
        public string Time { get; set; }
        public DateTime? EndDate { get; set; }
        public string TimeZone { get; set; }
        public DateTime? LastScheduledDateTime { get; set; }
        public string UserId { get; set; }
        public SystemUser User { get; set; }
    }
}
