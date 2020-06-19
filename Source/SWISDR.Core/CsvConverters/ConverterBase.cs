using CsvHelper;
using System;
using CsvHelper.TypeConversion;
using CsvHelper.Configuration;
using SWISDR.Core.Timetable;

namespace SWISDR.Core.CsvConverters
{
    public abstract class ConverterBase<T> : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            => text == TimetableLiterals.None ? default : ConvertFromString(text);
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            => value == null ? TimetableLiterals.None : ConvertToString((T)value);

        public abstract T ConvertFromString(string text);

        public virtual string ConvertToString(T value)
        {
            throw new NotSupportedException($"Convert {value} to string is not supported");
        }

    }
}
