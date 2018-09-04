using NanoSoft.Extensions;
using System;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => value?.DisplayName();

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => null;
    }
}