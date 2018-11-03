using JetBrains.Annotations;
using System;

namespace NanoSoft.Extensions
{
    [PublicAPI]
    public static class DateTimeExtensions
    {
        [NotNull]
        public static string ToDateString(this DateTime date)
            => date.ToString("yyyy-MM-dd");

        [NotNull]
        public static string ToTimeString(this DateTime date)
            => date.ToString("hh:mm tt");

        [NotNull]
        public static string ToDateTimeString(this DateTime date)
            => date.ToString("yyyy-MM-dd hh:mm tt");

        [CanBeNull]
        public static string ToDateString([CanBeNull] this DateTime? date)
            => date == null ? null : date.GetValueOrDefault().ToString("yyyy-MM-dd");

        public static bool IsValid(this DateTime date) => date.Year <= 2100 && date.Year >= 1900;

        public static DateTime ToDateTime([NotNull] this string dateTimeString)
        {
            TryConvert.ToDate(dateTimeString, out var dateTime);
            return dateTime;
        }

        [CanBeNull]
        public static DateTime? ToNullableDateTime([NotNull] this string dateTimeString)
        {
            if (TryConvert.ToDate(dateTimeString, out var dateTime))
                return dateTime;

            return null;
        }

        public static string ToTimeString(this TimeSpan time, string format = "tt hh:mm:ss")
        {
            if (time.Hours > 23)
                throw new ArgumentOutOfRangeException(nameof(time), @"time.Hours > 23");

            return new DateTime(1, 1, 1, time.Hours, time.Minutes, time.Seconds).ToString(format);
        }

        public static TimeSpan ToTimeSpan(this string timeString)
        {
            var date = timeString.ToDateTime();

            return new TimeSpan(date.Hour, date.Minute, date.Second);
        }

        public static TimeSpan? ToNullableTimeSpan(this string timeString)
        {
            var date = timeString.ToNullableDateTime();

            if (date == null)
                return null;

            return new TimeSpan(date.Value.Hour, date.Value.Minute, date.Value.Second);
        }
    }
}
