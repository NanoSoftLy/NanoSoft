using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace NanoSoft.Wpf.Converters
{
    public class RowToIndexConverter : MarkupExtension, IValueConverter
    {
        private static RowToIndexConverter _converter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => value is DataGridRow row ? row.GetIndex() + 1 : -1;

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
            => 0;

        public override object ProvideValue(IServiceProvider serviceProvider)
            => _converter ?? (_converter = new RowToIndexConverter());
    }
}
