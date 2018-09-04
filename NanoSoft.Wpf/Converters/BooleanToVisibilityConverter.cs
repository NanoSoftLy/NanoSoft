using System;
using System.Windows;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility Visibility { get; set; } = Visibility.Hidden;
        public bool Reverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter?.ToString().ToLower() == "collapsed")
                return value as bool? == !Reverse ? Visibility.Visible : Visibility.Collapsed;

            if (parameter?.ToString().ToLower() == "hidden")
                return value as bool? == !Reverse ? Visibility.Visible : Visibility.Hidden;

            return value as bool? == !Reverse ? Visibility.Visible : Visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => value as Visibility? == Visibility.Visible ? !Reverse : Reverse;
    }
}