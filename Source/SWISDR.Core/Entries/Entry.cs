using CsvHelper.Configuration;
using SWISDR.Core.CsvConverters;
using SWISDR.Core.Timetable;
using System;

namespace SWISDR.Core.Entries
{
    public class Entry
    {
        public int Number { get; set; }

        public TimeSpan? RealArrival { get; set; }
        public TimeSpan? RealDeparture { get; set; }
        public bool CallMade { get; set; }
        public bool SwdrEntryDone { get; set; }
        public string CustomNotes { get; set; }

        public Entry() { }
        public Entry(int number) => Number = number;
        public Entry(TimetableRecord record)
            => Number = record?.Number ?? throw new ArgumentNullException("record");

        public class Map : ClassMap<Entry>
        {
            public Map()
            {
                Map(e => e.Number);
                Map(e => e.RealArrival).TypeConverter<TimeConverter>();
                Map(e => e.RealDeparture).TypeConverter<TimeConverter>();
                Map(e => e.CallMade);
                Map(e => e.SwdrEntryDone);
                Map(e => e.CustomNotes);
            }
        }
    }
}
