using System;
using System.Windows;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!(parameter is string parameterString))
                return DependencyProperty.UnsetValue;

            if (System.Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            var parameterValue = System.Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(parameter is string parameterString))
                return DependencyProperty.UnsetValue;

            return System.Enum.Parse(targetType, parameterString);
        }
    }
}