using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SWISDR
{
    public class EntriesCollection : ObservableCollection<EntryViewModel>
    {
        private const int NotFound = -1;

        protected override void InsertItem(int index, EntryViewModel item)
        {
            item.PropertyChanged += OnItemPropertyChanged;
            base.InsertItem(GetProperIndex(index, item), item);
        }

        protected override void ClearItems()
        {
            foreach (var item in Items)
                item.PropertyChanged -= OnItemPropertyChanged;

            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            Items[index].PropertyChanged -= OnItemPropertyChanged;
            base.RemoveItem(index);
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var finished = sender as EntryViewModel;
            if (args.PropertyName != nameof(finished.AllSet) || !finished.AllSet)
                return;

            var finishedIndex = Items.IndexOf(finished);
            var notFinished = Items.First(e => !e.AllSet);
            var notFinishedIndex = Items.IndexOf(notFinished);

            if (notFinishedIndex > finishedIndex)
                return;

            Move(finishedIndex, notFinishedIndex);
        }

        private int GetProperIndex(int index, EntryViewModel entryViewModel)
        {
            if (!entryViewModel.AllSet)
                return SortByDepartureTime(index, entryViewModel);

            var notFinishedIndex = FindFirstNotFinishedIndex();
            if (notFinishedIndex == NotFound)
                return index;
            else
                return notFinishedIndex;
        }

        private int SortByDepartureTime(int index, EntryViewModel entryViewModel)
        {
            var item = Items.FirstOrDefault(item => !item.AllSet && item.Departure > entryViewModel.Departure);
            if (item == null)
                return index;

            var newIndex = IndexOf(item);
            if (item != null && newIndex < index)
                return newIndex;
            else
                return index;
        }

        private int FindFirstNotFinishedIndex()
        {
            var notFinished = Items.FirstOrDefault(e => !e.AllSet);
            if (notFinished == null)
                return NotFound;

            return Items.IndexOf(notFinished);
        }
    }
}
