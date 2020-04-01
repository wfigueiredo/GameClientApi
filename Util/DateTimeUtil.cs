using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Util
{
    public static class DateTimeUtil
    {
        public const string TIMEZONE_WINDOWS_SOUTH_AMERICA = "E. South America Standard Time";
        public const string TIMEZONE_UNIX_SOUTH_AMERICA = "America/Sao_Paulo";
        public static TimeZoneInfo TARGET_TIMEZONE => GetTargetTimeZone();

        public static DateTime FromUnixTimeStampToDateTime(this long unixTimeStamp)
        {
            return GetDateTimeConvertedIntoDefaultTimeZone(DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).LocalDateTime);
        }

        public static long FromDateTimeToUnixTimeStamp(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            var TargetDate = start.AddDays(daysToAdd);
            var TargetDateTime = new DateTime(TargetDate.Year, TargetDate.Month, TargetDate.Day, 0, 0, 0);
            return GetDateTimeConvertedIntoDefaultTimeZone(TargetDateTime);
        }

        public static DateTime GetDateTimeConvertedIntoDefaultTimeZone(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TARGET_TIMEZONE);
        }

        public static TimeZoneInfo GetTargetTimeZone()
        {
            return TimeZoneInfo.GetSystemTimeZones().ToList().Any(x => x.Id == TIMEZONE_WINDOWS_SOUTH_AMERICA) ?
                    TimeZoneInfo.FindSystemTimeZoneById(TIMEZONE_WINDOWS_SOUTH_AMERICA) :
                    TimeZoneInfo.FindSystemTimeZoneById(TIMEZONE_UNIX_SOUTH_AMERICA);
        }
    }
}
