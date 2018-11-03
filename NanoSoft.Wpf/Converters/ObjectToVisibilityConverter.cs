using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using NanoSoft.Extensions;

namespace NanoSoft.Wpf.Converters
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        public Visibility NotVisible { get; set; } = Visibility.Hidden;
        public bool IsVisibleIfNullOrDefault { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value.IsNullOrDefault() && !IsVisibleIfNullOrDefault ? NotVisible : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;


    }
}