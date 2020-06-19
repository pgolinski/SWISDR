using System;

namespace SWISDR.Core.CsvConverters
{
    public class TimeConverter : ConverterBase<TimeSpan?>
    {
        public override TimeSpan? ConvertFromString(string text) => TimeSpan.Parse(text);
        public override string ConvertToString(TimeSpan? value) => value.Value.ToString(@"hh\:mm");
    }
}
