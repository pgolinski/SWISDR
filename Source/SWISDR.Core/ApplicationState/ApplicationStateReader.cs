using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWISDR.Core.ApplicationState
{
    public interface IApplicationStateReader : IDisposable
    {
        Task<ApplicationState> Read();
    }

    public class ApplicationStateReader : IApplicationStateReader
    {
        private readonly Stream _stream;
        private readonly JsonSerializerOptions _options;
        private bool _disposed;

        public ApplicationStateReader(Stream stream, JsonSerializerOptions options)
        {
            _stream = stream;
            _options = options;
        }

        public Task<ApplicationState> Read()
            => JsonSerializer.DeserializeAsync<ApplicationState>(_stream, _options).AsTask();

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _stream.Dispose();
        }
    }
}
