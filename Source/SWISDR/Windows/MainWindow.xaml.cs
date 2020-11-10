using Microsoft.Win32;
using SWISDR.Core.ApplicationState;
using SWISDR.Core.Entries;
using SWISDR.Core.Timetable;
using SWISDR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SWISDR.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Func<string, Task<Timetable>> _loadTimetable;

        private readonly Func<TrainNumber> _windowFactory;
        private readonly IApplicationStateService _appStateService;
        private Timetable _timetable;
        private bool _manualCommitEdit;
        public EntriesCollection Entries { get; set; }

        public MainWindow(
            Func<string, Task<Timetable>> loadTimetableFunc,
            Func<TrainNumber> windowFactory,
            EntriesCollection entries,
            IApplicationStateService appStateService)
        {
            Entries = entries;
            InitializeComponent();
            _loadTimetable = loadTimetableFunc;
            _windowFactory = windowFactory;
            _appStateService = appStateService;
        }
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTimetable();
        }

        private void AddTrainBtn_Click(object sender, RoutedEventArgs e)
        {
            AddTrain();
        }

        private void EntriesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel || _manualCommitEdit)
                return;

            var datagrid = sender as DataGrid;

            _manualCommitEdit = true;
            datagrid.CommitEdit(DataGridEditingUnit.Row, true);
            _manualCommitEdit = false;
        }

        private async void LoadStateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appState = await _appStateService.Load(this);
                if (appState == null)
                    return;

                Entries.Clear();

                foreach (var entry in appState.Entries)
                    Entries.Add(new EntryViewModel(_timetable[entry.Number], entry));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void SaveStateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appState = new ApplicationState(Entries.Select(vm => vm.Model).ToList());
                if (await _appStateService.Save(appState, this))
                    MessageBox.Show("Stan zapisany");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                AddTrain();
                e.Handled = true;
            }
        }

        private void AddEntryFor(int number)
        {
            try
            {
                var entry = new Entry(_timetable[number]);
                Entries.Add(new EntryViewModel(_timetable[number], entry));
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show($"Nie znaleziono pociągu numer {number}");
            }
        }

        private async Task LoadTimetable()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Plik rozkładu (*.roz)|*.roz",
                CheckFileExists = true
            };

            if (dialog.ShowDialog(this) != true)
                return;

            _timetable = await _loadTimetable(dialog.FileName).ConfigureAwait(true);
            EntriesGrid.ItemsSource = Entries;
        }

        private void AddTrain()
        {
            var dialogWindow = _windowFactory();
            _ = dialogWindow.ShowDialog();
            if (!dialogWindow.Result)
                return;

            var number = dialogWindow.Number;

            var existing = Entries.FirstOrDefault(entry => entry.Number == number);
            if (existing == null)
            {
                AddEntryFor(number);
                return;
            }

            var currentTime = Entries.FindCurrentTime();
            var existingTime = existing.RealDeparture ?? existing.RealArrival;
            var difference = currentTime - existingTime ?? TimeSpan.Zero;
            if (difference < TimeSpan.Zero)
                difference = TimeSpan.FromDays(1) - difference.Duration();

            if (existingTime == null || difference < TimeSpan.FromHours(6))
            {
                MessageBox.Show($"Pociąg o numerze {number} znajduje się już na liście");
            }
            else
            {
                Entries.Remove(existing);
                AddEntryFor(dialogWindow.Number);
            }
        }

        private void FocusAndSelectCell(object sender, MouseButtonEventArgs e)
        {
            var cell = sender as DataGridCell;
            if (cell == null || cell.IsEditing || cell.IsReadOnly)
                return;
            if (!cell.IsFocused)
                cell.Focus();
            var dataGrid = FindVisualParent<DataGrid>(cell);
            if (dataGrid == null)
                return;

            if (dataGrid.SelectionUnit == DataGridSelectionUnit.FullRow)
                throw new InvalidOperationException("Full row selection is not supported");
            
            if (!cell.IsSelected)
                cell.IsSelected = true;
            else
            {
                dataGrid.BeginEdit(e);
                e.Handled = true;
            }
        }

        private static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            var parent = element;
            while (parent != null)
            {
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                    return correctlyTyped;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = e.Source as MenuItem;
            var entry = menuItem?.DataContext as EntryViewModel;
            if (entry != null)
                Entries.Remove(entry);
        }
    }
}
