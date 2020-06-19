using System.Collections.Generic;

namespace SWISDR.Core.Timetable
{
    public class Timetable
    {
        private Dictionary<int, TimetableRecord> _records;

        public IEnumerable<TimetableRecord> Records => _records.Values;
        public Timetable(IEnumerable<TimetableRecord> records)
        {
            _records = new Dictionary<int, TimetableRecord>();
            foreach (var record in records)
                _records[record.Number] = record; //duplicates will be overriden
        }

        public TimetableRecord this[int number] => _records[number];
    }
}
