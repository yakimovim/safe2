using Safe.Core.Domain;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Safe.Utilities
{
    [ValueConversion(typeof(FieldTypes), typeof(ImageSource))]
    public class FieldTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fieldType = (FieldTypes)value;

            switch(fieldType)
            {
                case FieldTypes.SingleLineText:
                    return new BitmapImage(new Uri("/Images/singleLine32.png", UriKind.RelativeOrAbsolute));
                case FieldTypes.MultiLineText:
                    return new BitmapImage(new Uri("/Images/multiLine32.png", UriKind.RelativeOrAbsolute));
                case FieldTypes.Password:
                    return new BitmapImage(new Uri("/Images/password32.png", UriKind.RelativeOrAbsolute));
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
