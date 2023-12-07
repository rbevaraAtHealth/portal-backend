using CodeMatcherV2Api.EntityFrameworkCore;
using NCrontab;
using System.Globalization;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CodeMatcher.Api.V2.Middlewares.CommonHelper
{
    public class ConvertTimeZoneHelper
    {
        private readonly CodeMatcherDbContext context;
        public ConvertTimeZoneHelper(CodeMatcherDbContext _context)
        {
            context = _context;
        }
        public async Task<string> ConverTimeZone(string cronExpression)
        {
            string operatingSystem = RuntimeInformation.OSDescription;
            TimeZoneInfo tzInfo;

            tzInfo = (operatingSystem.Contains("Microsoft Windows"))
                   ? TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
                   : TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");

            var cronExpressionPST = cronExpression; // PST cron expression

            // Parse the cron expression to get the schedule
            var schedule = CrontabSchedule.Parse(cronExpressionPST);

            // Get the current UTC time
            var currentUTC = DateTime.UtcNow;

            // Convert the current UTC time to PST
            var currentPST = TimeZoneInfo.ConvertTimeFromUtc(currentUTC, tzInfo);

            // Calculate the next occurrence in PST
            var nextOccurrencePST = schedule.GetNextOccurrence(currentPST);

            // Convert the next occurrence from PST to UTC
            var nextOccurrenceUTC = TimeZoneInfo.ConvertTimeToUtc(nextOccurrencePST, tzInfo);

            // Prepare arrays to maintain wildcards
            var partsPST = cronExpressionPST.Split(' ');
            var partsUTC = nextOccurrenceUTC.ToString("mm HH dd MM ddd", System.Globalization.CultureInfo.InvariantCulture).Split(' ');

            // Map day of the week name to the desired numbering (Monday: 1, ..., Sunday: 7)
            var dayOfWeekNumber = ((int)nextOccurrenceUTC.DayOfWeek + 6) % 7 + 1;

            // Replace day of the week part in UTC cron expression with the number
            partsUTC[4] = dayOfWeekNumber.ToString();

            // Replace corresponding parts with '' if the original PST cron expression had ''
            for (int i = 0; i < partsPST.Length; i++)
            {
                if (partsPST[i] == "*")
                {
                    partsUTC[i] = "*";
                }
            }

            // Join parts to form the final cron expression in UTC
            var cronScheduleUTC = string.Join(" ", partsUTC);
            return cronScheduleUTC.ToString();
        }

    }
}
