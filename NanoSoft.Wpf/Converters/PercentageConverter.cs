using System;
using System.Windows.Data;

namespace NanoSoft.Wpf.Converters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            var parentValue = System.Convert.ToDouble(value);

            if (parameter != null)
            {
                var parameterValue = System.Convert.ToDouble(parameter);

                if (parentValue < 1000 && parentValue >= 800)
                    parameterValue = parameterValue * 2;

                if (parentValue < 800 && parentValue >= 300)
                    parameterValue = parameterValue * 3;

                if (parentValue < 300)
                    parameterValue = parameterValue * 4;

                if (parameterValue > 1)
                    parameterValue = 1;

                return parentValue * parameterValue;
            }

            var percent = 0.25;

            if (parentValue < 1200 && parentValue >= 1000)
                percent = 0.33;

            if (parentValue < 1000 && parentValue >= 800)
                percent = 0.5;

            if (parentValue < 800)
                percent = 1;

            return parentValue * percent;

        }

        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
