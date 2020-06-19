using Microsoft.Win32;
using SWISDR.Core.ApplicationState;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SWISDR.Services
{
    public interface IApplicationStateService
    {
        Task<ApplicationState> Load(Window parent);
        Task Save(ApplicationState appState, Window parent, bool forceChooseFile = false);
    }

    public class ApplicationStateService : IApplicationStateService
    {
        private const string StateFileExtension = "state";
        private readonly Func<string, IApplicationStateWriter> _appStateWriterFactory;
        private readonly Func<string, IApplicationStateReader> _appStateReaderFactory;

        private string _latestPath;

        public ApplicationStateService(
            Func<string, IApplicationStateWriter> appStateWriterFactory,
            Func<string, IApplicationStateReader> appStateReaderFactory)
        {
            _appStateWriterFactory = appStateWriterFactory;
            _appStateReaderFactory = appStateReaderFactory;
        }

        public Task<ApplicationState> Load(Window parent)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = StateFileExtension;
            if (dialog.ShowDialog(parent) != true)
                return Task.FromResult((ApplicationState)null);

            _latestPath = dialog.FileName;
            var reader = _appStateReaderFactory(_latestPath);

            var task = reader.Read();
            task.ContinueWith(_ => reader.Dispose());
            return task;
        }

        public Task Save(ApplicationState appState, Window parent, bool forceChooseFile = false)
        {
            if (_latestPath == null || forceChooseFile)
            {
                var dialog = new SaveFileDialog();
                dialog.DefaultExt = StateFileExtension;
                if (dialog.ShowDialog(parent) != true)
                    return Task.CompletedTask;

                _latestPath = dialog.FileName;
            }

            var writer = _appStateWriterFactory(_latestPath);

            var task = writer.Write(appState);
            task.ContinueWith(_ => writer.Dispose());
            return task;
        }
    }
}
