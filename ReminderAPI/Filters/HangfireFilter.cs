using Hangfire.Dashboard;

namespace ReminderAPI.Filters
{
    public class HangfireFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
