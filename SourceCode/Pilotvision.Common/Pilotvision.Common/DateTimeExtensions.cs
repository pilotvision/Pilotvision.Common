using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pilotvision.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime BeginOfMonth(this DateTime value)
        {
            return value.AddDays(-value.Day + 1);
        }

        public static DateTime EndOfMonth(this DateTime value)
        {
            return BeginOfMonth(value.AddMonths(1)).AddDays(-1);
        }

        public static DateTime GetLocalDateTime(this DateTime dateTime, string destinationTimeZoneId)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneId));
        }

        public static DateTime GetTokyoDateTime(this DateTime local)
        {
            return local.GetLocalDateTime("Tokyo Standard Time");
        }

        public static string GetWeekJpnShortString(this DateTime value)
        {
            if (value.DayOfWeek == DayOfWeek.Monday)
            {
                return "月";
            }
            else if (value.DayOfWeek == DayOfWeek.Tuesday)
            {
                return "火";
            }
            else if (value.DayOfWeek == DayOfWeek.Wednesday)
            {
                return "水";
            }
            else if (value.DayOfWeek == DayOfWeek.Thursday)
            {
                return "木";
            }
            else if (value.DayOfWeek == DayOfWeek.Friday)
            {
                return "金";
            }
            else if (value.DayOfWeek == DayOfWeek.Saturday)
            {
                return "土";
            }
            else
            {
                return "日";
            }
        }
    }
}
