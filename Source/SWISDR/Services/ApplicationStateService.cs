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
        Task<bool> Save(ApplicationState appState, Window parent, bool forceChooseFile = false);
    }

    public class ApplicationStateService : IApplicationStateService
    {
        private const string FileDialogFilter = "Plik stanu (*.state)|*.state";
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

        public async Task<ApplicationState> Load(Window parent)
        {
            var dialog = new OpenFileDialog
            {
                Filter = FileDialogFilter,
                CheckFileExists = true
            };

            if (dialog.ShowDialog(parent) != true)
                return null;

            _latestPath = dialog.FileName;
            using var reader = _appStateReaderFactory(_latestPath);
            return await reader.Read();
        }

        public async Task<bool> Save(ApplicationState appState, Window parent, bool forceChooseFile = false)
        {
            if (_latestPath == null || forceChooseFile)
            {
                var dialog = new SaveFileDialog()
                {
                    Filter = FileDialogFilter
                };

                if (dialog.ShowDialog(parent) != true)
                    return false;

                _latestPath = dialog.FileName;
            }

            using var writer = _appStateWriterFactory(_latestPath);
            await writer.Write(appState);
            return true;
        }
    }
}
