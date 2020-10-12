using SWISDR.Core.Entries;
using SWISDR.Core.Timetable;
using System;
using System.ComponentModel;

namespace SWISDR
{

    public class EntryViewModel : INotifyPropertyChanged
    {
        private readonly TimetableRecord _record;
        private readonly Entry _entry;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Number => _record.Number;
        public string From => _record.From;
        public string To => _record.To;
        public TimeSpan? Arrival => _record.Arrival;
        public TimeSpan? Departure => _record.Departure;
        public string Category => _record.Category;
        public string Track => _record.Track;
        public string Notes => _record.Notes;
        public bool StartsRun => _record.StartsRun;
        public bool EndsRun => _record.EndsRun;

        public TimeSpan? RealArrival
        {
            get => _entry.RealArrival;
            set
            {
                _entry.RealArrival = value;
                NotifyPropertyChanged(nameof(RealArrival));
                NotifyPropertyChanged(nameof(Arriving));
                NotifyPropertyChanged(nameof(AllSet));
            }
        }
        public TimeSpan? RealDeparture
        {
            get => _entry.RealDeparture;
            set
            {
                _entry.RealDeparture = value;
                NotifyPropertyChanged(nameof(RealDeparture));
                NotifyPropertyChanged(nameof(TrainToRun));
                NotifyPropertyChanged(nameof(AllSet));
            }
        }

        public string CustomNotes
        {
            get => _entry.CustomNotes;
            set
            {
                _entry.CustomNotes = value;
                NotifyPropertyChanged(nameof(CustomNotes));
            }
        }

        public bool CallMade
        {
            get => _entry.CallMade;
            set
            {
                _entry.CallMade = value;
                NotifyPropertyChanged(nameof(CallMade));
                NotifyPropertyChanged(nameof(AllSet));
            }
        }

        public bool SwdrEntryDone
        {
            get => _entry.SwdrEntryDone;
            set
            {
                _entry.SwdrEntryDone = value;
                NotifyPropertyChanged(nameof(SwdrEntryDone));
                NotifyPropertyChanged(nameof(AllSet));
            }
        }

        public bool StartsHere => Arrival == null;
        public bool EndsHere => Departure == null;

        public bool TrainToRun => StartsHere && RealDeparture == null;
        public bool Arriving => Arrival != null && RealArrival == null;
        public bool AllSet => (RealDeparture != null || Departure == null && RealArrival != null) && CallMade && SwdrEntryDone;


        public EntryViewModel(TimetableRecord record, Entry entry)
        {
            _record = record;
            _entry = entry;
        }

        public Entry Model => new Entry
        {
            Number = _entry.Number,
            CallMade = _entry.CallMade,
            CustomNotes = _entry.CustomNotes,
            RealArrival = _entry.RealArrival,
            RealDeparture = _entry.RealDeparture,
            SwdrEntryDone = _entry.SwdrEntryDone
        };

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
