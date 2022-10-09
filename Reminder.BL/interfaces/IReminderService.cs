using Reminder.BL.Helpers;
using Reminder.Entities.DTO;
using System.Threading.Tasks;

namespace Reminder.BL.interfaces
{
    public interface IReminderService
    {
        Task ScheduleReminder();
        void SendReminder(Notification notification);
        Task<Response<string>> SaveReminder(ReminderDTO reminder);
        
    }
}
