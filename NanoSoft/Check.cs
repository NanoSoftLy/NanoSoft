using JetBrains.Annotations;
using NanoSoft.Extensions;
using NanoSoft.Resources;
using System;

namespace NanoSoft
{
    [PublicAPI]
    public static class Check
    {
        public static void NotNull([CanBeNull] object obj, [NotNull][InvokerParameterName] string parameterName)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }

        public static void IsValidDate(DateTime date, [NotNull][InvokerParameterName] string parameterName)
        {
            if (!date.IsValid())
                throw new ArgumentOutOfRangeException(parameterName, SharedMessages.Check_IsValidDate);
        }

        public static void NotEmpty(string value, [NotNull][InvokerParameterName] string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(SharedMessages.Check_NotEmpty, parameterName);
        }

        public static void MoreThanZero(decimal number, [NotNull][InvokerParameterName] string parameterName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(parameterName, SharedMessages.Check_MoreThanZero);
        }

        public static void ZeroOrMore(decimal number, [NotNull][InvokerParameterName] string parameterName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(parameterName, SharedMessages.Check_ZeroOrMore);
        }

        public static void NotZero(decimal number, [NotNull][InvokerParameterName] string parameterName)
        {
            if (number == 0)
                throw new ArgumentOutOfRangeException(parameterName, SharedMessages.Check_NotZero);
        }
    }
}