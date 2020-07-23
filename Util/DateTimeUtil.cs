using System;
using System.Linq;

namespace GameClientApi.Util
{
    public static class DateTimeUtil
    {
        public const string DATE_TIME_TZ_FORMAT = "yyyy-MM-ddTHH:mm:ss";
        public const string DATE_TIME_TIMESTAMPED_LABEL_FORMAT = "yyyyMMdd-HHmmss";

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
            int daysToAdd = ((int)day - (int)start.DayOfWeek - 7) % 7;
            var TargetDate = (daysToAdd != 0) ? start.AddDays(daysToAdd) : start.AddDays(7);
            var TargetDateTime = new DateTime(TargetDate.Year, TargetDate.Month, TargetDate.Day, 0, 0, 0);
            return GetDateTimeConvertedIntoDefaultTimeZone(TargetDateTime);
        }

        public static DateTime GetDateTimeConvertedIntoDefaultTimeZone(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TARGET_TIMEZONE);
        }

        public static string ConvertToString(DateTime dt, string pattern)
        {
            return dt.ToString(pattern);
        }

        public static TimeZoneInfo GetTargetTimeZone()
        {
            return TimeZoneInfo.GetSystemTimeZones().ToList().Any(x => x.Id == TIMEZONE_WINDOWS_SOUTH_AMERICA) ?
                    TimeZoneInfo.FindSystemTimeZoneById(TIMEZONE_WINDOWS_SOUTH_AMERICA) :
                    TimeZoneInfo.FindSystemTimeZoneById(TIMEZONE_UNIX_SOUTH_AMERICA);
        }
    }
}
