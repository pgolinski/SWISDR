using CsvHelper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWISDR.Core.Timetable
{
    public interface ITimetableReader : IDisposable
    {
        Task<Timetable> Read();
    }

    public class TimetableReader : ITimetableReader
    {
        private readonly IReader _reader;
        private bool _disposed;

        public TimetableReader(IReader reader)
        {
            _reader = reader;
        }

        public async Task<Timetable> Read()
        {
            var records = new List<TimetableRecord>();
            while (await _reader.ReadAsync())
                records.Add(_reader.GetRecord<TimetableRecord>());

            return new Timetable(records);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _reader.Dispose();
        }
    }
}
