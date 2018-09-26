using System;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public IntToStringReturnValue ReturnValue { get; set; } = IntToStringReturnValue.Null;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => value?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var parsed = int.TryParse(value?.ToString(), out var intVal);

            if (parsed)
                return intVal;

            Console.WriteLine($"ConvertBack to int failed in {nameof(IntToStringConverter)}, value = {value}");
            return ReturnValue == IntToStringReturnValue.Zero ? 0 : (int?)null;
        }
    }


    public enum IntToStringReturnValue
    {
        Null,
        Zero,
    }
}