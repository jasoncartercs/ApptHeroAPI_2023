using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Common.Utilities
{
    public static class TimeZoneHelper
    {
        public static DateTime ConvertTimeToUsersTimeZone(DateTime utcDate, string timeZoneId)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Utc;
            try
            {
                localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                return utcDate;
            }
            utcDate = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, localTimeZone);
            return localTime;
        }

        public static DateTime ConvertTimeToUtc(DateTime date, string timeZoneId)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Utc;
            try
            {
                localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                return date;
            }
            return TimeZoneInfo.ConvertTimeToUtc(date, localTimeZone);
        }



        public static TimeSpan? ConvertTimeToUsersTimeZone(TimeSpan? timespan, string timeZoneId)
        {
            if (timespan == null)
            {
                return null;
            }
            DateTime dateTime = new DateTime(DateTime.Now.Year,
                                                   DateTime.Now.Month, DateTime.Now.Day, timespan.Value.Hours,
                                                   timespan.Value.Minutes, timespan.Value.Seconds);
            TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime dateTimeInTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timezone);
            return dateTimeInTimeZone.TimeOfDay;
        }
    }
}
