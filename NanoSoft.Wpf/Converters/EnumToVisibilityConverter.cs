using System;
using System.Windows;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        public Visibility Visibility { get; set; } = Visibility.Hidden;
        public bool Reverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!(parameter is string parameterString))
                return Visibility;

            if (System.Enum.IsDefined(value.GetType(), value) == false)
                return Visibility;

            var parameterValue = System.Enum.Parse(value.GetType(), parameterString);

            if (Reverse)
                return parameterValue.Equals(value) ? Visibility : Visibility.Visible;

            return parameterValue.Equals(value) ? Visibility.Visible : Visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => DependencyProperty.UnsetValue;
    }
}