using CsvHelper.Configuration;
using SWISDR.Core.CsvConverters;
using System;
using System.Linq;
using System.Reflection;
using StringConverter = SWISDR.Core.CsvConverters.StringConverter;

namespace SWISDR.Core.Timetable
{
    public class TimetableRecord
    {
        public RunDays RunDays { get; set; }
        public int Number { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public TimeSpan? AdjacentDeparture { get; set; }
        public TimeSpan? Arrival { get; set; }
        public TimeSpan? Departure { get; set; }
        public TimeSpan? AdjacentArrival { get; set; }
        public string Category { get; set; }
        public string Track { get; set; }
        public string Notes { get; set; }

        public bool StartsRun => Arrival == null;
        public bool EndsRun => Departure == null;

        public class Map : ClassMap<TimetableRecord>
        {
            public Map()
            {
                Map(t => t.RunDays).Index(FieldIndex.Days).TypeConverter<RunDaysConverter>();
                Map(t => t.Number).Index(FieldIndex.Number);
                Map(t => t.From).Index(FieldIndex.From).TypeConverter<UpperCaseConverter>();
                Map(t => t.To).Index(FieldIndex.To).TypeConverter<UpperCaseConverter>();
                Map(t => t.AdjacentDeparture).Index(FieldIndex.AdjacentDeparture).TypeConverter<TimeConverter>();
                Map(t => t.Arrival).Index(FieldIndex.Arrival).TypeConverter<TimeConverter>();
                Map(t => t.Departure).Index(FieldIndex.Departure).TypeConverter<TimeConverter>();
                Map(t => t.AdjacentArrival).Index(FieldIndex.AdjacentArrival).TypeConverter<TimeConverter>();
                Map(t => t.Category).Index(FieldIndex.Category).TypeConverter<CategoryConverter>();
                Map(t => t.Track).Index(FieldIndex.Track).TypeConverter<StringConverter>();
                Map(t => t.Notes).Index(FieldIndex.Notes).TypeConverter<StringConverter>();
            }
        }

        public override string ToString()
        {
            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return string.Join(", ", props.Select(p => $"{p.Name}: {p.GetValue(this)}"));
        }
    }
}
