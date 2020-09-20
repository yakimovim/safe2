using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Safe.Utilities
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = System.Convert.ToBoolean(value);

            if(parameter is string strParameter)
            {
                if("Invert".Equals(strParameter, StringComparison.OrdinalIgnoreCase))
                {
                    boolValue = !boolValue;
                }
            }

            return boolValue == true
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
