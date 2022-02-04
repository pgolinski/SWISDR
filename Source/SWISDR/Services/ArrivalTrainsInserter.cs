using SWISDR.Core.ApplicationState;
using SWISDR.Core.Timetable;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWISDR.Services
{
    public class ArrivalTrainsInserter
    {
        private readonly EntriesCollection _entries;
        private readonly Timetable _timetable;

        public ArrivalTrainsInserter(EntriesCollection entries, Timetable timetable)
        {
            _entries = entries;
            _timetable = timetable;
        }

        public void Enable()
        {
            _entries.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var currentTime = _entries.FindCurrentTime();

            if (currentTime == null)
                return;

            var nextHour = currentTime.Value.Add(TimeSpan.FromHours(1));

            _timetable.Records.Where(record => record.StartsRun && (record.Departure - currentTime.Value))
        }
    }
}
