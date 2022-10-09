using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reminder.BL.Helpers;
using Reminder.BL.interfaces;
using Reminder.Entities;
using Reminder.Entities.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneConverter;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Reminder.BL.implementations
{
    public class ReminderService : IReminderService
    {
        private readonly ReminderContext dataContext;
        private readonly ILogger<ReminderService> logger;
        private readonly TwilioConfig twilioConfig;
        public ReminderService(ReminderContext dataContext, ILogger<ReminderService> logger, IOptions<TwilioConfig> twilioConfig)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.twilioConfig = twilioConfig.Value;
        }
        public async Task ScheduleReminder()
        {
            var reminders = await dataContext.Reminders.Include(x => x.User)
                                                        .Where(x => DateTime.Now.Date <= x.EndDate.Value.Date)
                                                        .ToListAsync();
            var notification = new Notification();
            var today = DateTime.UtcNow.Date;
            foreach (var reminder in reminders)
            {
                if (reminder.LastScheduledDateTime == null || reminder.LastScheduledDateTime.Value.Date < today)
                {
                    try
                    {
                        // Initializations
                        notification.Message = string.Format(reminder.Message, reminder.User.Name);
                        notification.PhoneNumber = reminder.User.CountryCode + reminder.User.CityCode + reminder.User.PhoneNumber;

                        if (reminder.Time != null)
                        {
                            string[] timeArray = reminder.Time.Split(':');

                            var scheduleDate = new DateTime(today.Year,
                                today.Month,
                                today.Day,
                                int.Parse(timeArray[0]), // hour
                                int.Parse(timeArray[1]), // minute
                                int.Parse(timeArray[2]),  // second 
                                DateTimeKind.Unspecified
                            );

                            var localTime = TimeZoneInfo.ConvertTimeFromUtc(scheduleDate, TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(reminder.TimeZone)));
                            PrepareReminder(localTime, reminder, notification);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Something went wrong for scheduling job ReminderId:{reminder.Id}");
                        logger.LogError(e.ToString());
                        continue;
                    }
                }
                else
                {
                    logger.LogInformation($"Reminder Else LastScheduledDateTime" + reminder.LastScheduledDateTime);
                }
            }
            await dataContext.SaveChangesAsync();
            logger.LogInformation($"Ended the Schedule reminder");

        }
        public void SendReminder(Notification notification)
        {
            SendSMS(notification.Message, notification.PhoneNumber);
        }
        public async Task<Response<string>> SaveReminder(ReminderDTO reminder)
        {
            var reminderIns = new Reminder.Entities.Models.Reminder()
            {
                Message = reminder.Message,
                Time = reminder.Time,
                EndDate = reminder.EndDate,
                TimeZone = reminder.TimeZone,
                UserId = reminder.UserId,
                CreatedDate = DateTime.Now,
                CreatedBy = new Guid(reminder.UserId),

            };
            await dataContext.Reminders.AddAsync(reminderIns);
            await dataContext.SaveChangesAsync();
            return new Response<string>() { Message = "Reminder saved successfully" };
        }

        #region Private region
        private void PrepareReminder(DateTime localTime, Reminder.Entities.Models.Reminder reminder, Notification notification)
        {

            reminder.LastScheduledDateTime = TimeZoneInfo.ConvertTimeToUtc(localTime, TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(reminder.TimeZone)));
            var dateTimeOffSet = new DateTimeOffset(reminder.LastScheduledDateTime.Value);

            BackgroundJob.Schedule<IReminderService>(x => x.SendReminder(notification), dateTimeOffSet);
            logger.LogInformation($"Job Scheduled for {reminder.UserId} at {dateTimeOffSet}");

        }

        private void SendSMS(string messageTxt, string phoneNumber)
        {
            TwilioClient.Init(twilioConfig.SID, twilioConfig.AuthToken);
            try
            {
                var message = MessageResource.Create(
                        body: messageTxt,
                        from: new Twilio.Types.PhoneNumber(twilioConfig.PhoneNumber),
                        to: new Twilio.Types.PhoneNumber(phoneNumber)
                 );
                logger.LogInformation("Message ID has been successfully sent");
            }
            catch { logger.LogError("Message was not sent due to following exception"); }
        }


        #endregion
    }
}
