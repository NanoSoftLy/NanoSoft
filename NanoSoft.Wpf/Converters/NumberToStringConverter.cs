using System;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class NumberToStringConverter : IValueConverter
    {
        public NumberToStringReturnValue ReturnValue { get; set; } = NumberToStringReturnValue.Null;
        public NumberToStringType NumberType { get; set; } = NumberToStringType.Int;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => value?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (NumberType)
            {
                case NumberToStringType.Int:
                    if (int.TryParse(value?.ToString(), out var intVal))
                        return intVal;
                    break;

                case NumberToStringType.Long:
                    if (long.TryParse(value?.ToString(), out var longVal))
                        return longVal;
                    break;

                case NumberToStringType.Double:
                    if (double.TryParse(value?.ToString(), out var doubleVal))
                        return doubleVal;
                    break;

                case NumberToStringType.Decimal:
                    if (decimal.TryParse(value?.ToString(), out var decimalVal))
                        return decimalVal;
                    break;

                default:
                    break;
            }

            Console.WriteLine($"ConvertBack to int failed in {nameof(NumberToStringConverter)}, value = {value}");
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