using Autofac;
using CsvHelper;
using SWISDR.Core;
using SWISDR.Core.Timetable;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SWISDR
{
    class Program
    {
        static async Task Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule());

            var container = builder.Build();

            var timetableTaskFactory = container.Resolve<Func<string, Task<Timetable>>>();
            var timetableTask = timetableTaskFactory(@"C:\Users\przemekg\Downloads\MaKu-rozklady\Liskow\maku_Liskow.roz");
            try
            {
                var timetable = await timetableTask.ConfigureAwait(false);
                foreach (var record in timetable.Records)
                    Console.WriteLine(record);
            }
            catch (ReaderException e)
            {
                Console.WriteLine($"Unable to read timetable. {e}\nContext: ({e.ReadingContext.Row},{e.ReadingContext.CurrentIndex}): {e.ReadingContext.RawRecord}");
            }
        }
    }
}
