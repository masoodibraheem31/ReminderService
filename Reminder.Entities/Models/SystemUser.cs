using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Reminder.Entities.Models
{
    public class SystemUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public override string PhoneNumber { get; set; }
        public List<Reminder> Reminders { get; set; }
    }

    public class SystemRole : IdentityRole
    {
        public bool IsDeleted { get; set; } = false;
    }

    public class SystemUserRole : IdentityUserRole<string>
    {
        public bool IsDeleted { get; set; } = false;
    }
}
