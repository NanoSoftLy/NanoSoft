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
    }
}
