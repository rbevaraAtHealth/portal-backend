using CodeMatcherV2Api.EntityFrameworkCore;
using NCrontab;
using System.Globalization;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CodeMatcher.Api.V2.Middlewares.CommonHelper
{
    public static class ConvertTimeZoneHelper
    {
        public static string ConvertTimeZone(string cronExpression)
        {
            string operatingSystem = RuntimeInformation.OSDescription;
            TimeZoneInfo tzInfo;

            tzInfo = (operatingSystem.Contains("Microsoft Windows"))
                   ? TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
                   : TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");

            var cronExpressionPST = cronExpression; 
            var schedule = CrontabSchedule.Parse(cronExpressionPST);
            var currentUTC = DateTime.UtcNow;
            var currentPST = TimeZoneInfo.ConvertTimeFromUtc(currentUTC, tzInfo);
            var nextOccurrencePST = schedule.GetNextOccurrence(currentPST);
            var nextOccurrenceUTC = TimeZoneInfo.ConvertTimeToUtc(nextOccurrencePST, tzInfo);
            var partsPST = cronExpressionPST.Split(' ');
            var partsUTC = nextOccurrenceUTC.ToString("mm HH dd MM ddd", System.Globalization.CultureInfo.InvariantCulture).Split(' ');
            var dayOfWeekNumber = ((int)nextOccurrenceUTC.DayOfWeek + 6) % 7 + 1;
            partsUTC[4] = dayOfWeekNumber.ToString();
            for (int i = 0; i < partsPST.Length; i++)
            {
                if (partsPST[i] == "*")
                {
                    partsUTC[i] = "*";
                }
            }
            var cronScheduleUTC = string.Join(" ", partsUTC);
            return cronScheduleUTC.ToString();
        }

    }
}
