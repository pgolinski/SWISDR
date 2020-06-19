using SWISDR.Core.Entries;
using SWISDR.Core.Timetable;
using System.Collections.Generic;

namespace SWISDR.Core.ApplicationState
{
    public class ApplicationState
    {
        public List<TimetableRecord> NotPlannedTrains { get; set; }
        public List<Entry> Entries { get; set; }

        public ApplicationState()
        {
            Entries = new List<Entry>();
            NotPlannedTrains = new List<TimetableRecord>();
        }

        public ApplicationState(List<Entry> entries, List<TimetableRecord> notPlanned = null)
        {
            Entries = entries ?? new List<Entry>();
            NotPlannedTrains = notPlanned ?? new List<TimetableRecord>();
        }
    }
}
