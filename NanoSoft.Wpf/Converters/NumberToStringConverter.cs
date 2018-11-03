using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class NumberToStringConverter : IValueConverter
    {
        public NumberToStringReturnValue ReturnValue { get; set; } = NumberToStringReturnValue.Null;
        public NumberToStringType NumberType { get; set; } = NumberToStringType.Int;

        public KeyValuePair<object, string>? KeyValuePair { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (KeyValuePair != null && KeyValuePair.Value.Key.Equals(value))
            {
                var val = KeyValuePair.Value.Value;
                KeyValuePair = null;
                return val;
            }

            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            KeyValuePair = null;
            object returnedValue = null;

            switch (NumberType)
            {
                case NumberToStringType.Int:
                    if (int.TryParse(value?.ToString(), out var intVal))
                        returnedValue = intVal;
                    break;

                case NumberToStringType.Long:
                    if (long.TryParse(value?.ToString(), out var longVal))
                        returnedValue = longVal;
                    break;

                case NumberToStringType.Double:
                    if (double.TryParse(value?.ToString().Trim('.'), out var doubleVal))
                        returnedValue = doubleVal;
                    break;

                case NumberToStringType.Decimal:
                    if (decimal.TryParse(value?.ToString().Trim('.'), out var decimalVal))
                        returnedValue = decimalVal;
                    break;
            }

            if (returnedValue != null)
            {
                KeyValuePair = new KeyValuePair<object, string>(returnedValue, value?.ToString());
                return returnedValue;
            }

            Console.WriteLine($@"ConvertBack to int failed in {nameof(NumberToStringConverter)}, value = {value}");
            return ReturnValue == NumberToStringReturnValue.Zero ? 0 : (object)null;
        }
    }

    public enum NumberToStringReturnValue
    {
        Null,
        Zero,
    }

    public enum NumberToStringType
    {
        Int,
        Long,
        Double,
        Decimal
    }
}