using System;
using System.Globalization;
using System.Windows.Data;

namespace SWISDR.ValueConverters
{
    public class TimespanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan?)value;
            return timeSpan?.ToString(@"hh\:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sValue = (string)value;
            if (string.IsNullOrWhiteSpace(sValue))
                return null;

            if (sValue.Contains(":"))
                return Parse(sValue.PadLeft(5, '0'));
            else
                return Parse(sValue.PadLeft(4, '0').Insert(2, ":").ToString());
        }

        private TimeSpan? Parse(string s) => TimeSpan.TryParse(s, out var result) ? (TimeSpan?)result : null;
    }
}
