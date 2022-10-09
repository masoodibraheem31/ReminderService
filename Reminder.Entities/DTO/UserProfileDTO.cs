using System;

namespace Reminder.Entities.DTO
{
    public class UserProfileDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
