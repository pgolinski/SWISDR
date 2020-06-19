using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWISDR.Core.ApplicationState
{
    public interface IApplicationStateWriter : IDisposable
    {
        Task Write(ApplicationState appState);
    }

    public class ApplicationStateWriter : IDisposable, IApplicationStateWriter
    {
        private readonly TextWriter _writer;
        private readonly JsonSerializerOptions _options;
        private bool _disposed;

        public ApplicationStateWriter(Stream writer, JsonSerializerOptions options)
        {
            _writer = new StreamWriter(writer);
            _options = options;
        }

        public async Task Write(ApplicationState appState)

        {
            var json = JsonSerializer.Serialize(appState, _options);
            await _writer.WriteAsync(json);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _writer.Dispose();
        }
    }
}
