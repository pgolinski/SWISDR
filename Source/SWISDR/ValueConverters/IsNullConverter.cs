using System;
using System.Globalization;
using System.Windows.Data;

namespace SWISDR.ValueConverters
{
    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value == null);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("IsNullConverter can only be used OneWay.");
        }
    }
}
