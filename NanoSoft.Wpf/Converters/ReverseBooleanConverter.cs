using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class ReverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTrue)
                return !isTrue;

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTrue)
                return !isTrue;

            return DependencyProperty.UnsetValue;
        }
    }
}