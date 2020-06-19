using CsvHelper;
using System;
using System.Collections.Generic;

namespace SWISDR.Core.Entries
{
    public interface IEntriesWriter : IDisposable
    {
        void Write(IEnumerable<Entry> entries);
    }

    public class EntriesCsvWriter : IDisposable, IEntriesWriter
    {
        private readonly IWriter _writer;
        private readonly bool _leaveOpen;
        private bool _disposed;

        public EntriesCsvWriter(IWriter writer, bool leaveOpen = false)
        {
            _writer = writer;
            _leaveOpen = leaveOpen;
        }

        public void Write(IEnumerable<Entry> entries)
        {
            _writer.WriteRecords(entries);
        }
        public void Dispose()
        {
            if (_leaveOpen || _disposed)
                return;

            _disposed = true;
            _writer.Dispose();
        }
    }
}
