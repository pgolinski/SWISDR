using Autofac;
using Autofac.Core;
using Autofac.Features.OwnedInstances;
using CsvHelper;
using CsvHelper.Configuration;
using SWISDR.Core.ApplicationState;
using SWISDR.Core.Entries;
using SWISDR.Core.Timetable;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWISDR.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(CsvReader)
                .AsImplementedInterfaces()
                .ExternallyOwned();
            builder.Register(CsvWriter)
                .AsImplementedInterfaces()
                .ExternallyOwned();
            builder.RegisterType<TimetableReader>()
                .AsImplementedInterfaces();
            builder.RegisterType<EntriesCsvWriter>()
                .AsImplementedInterfaces()
                .ExternallyOwned();
            builder.Register(ReadTimetable);
            builder.Register(TimetableFileEncoding);

            builder.RegisterType<ApplicationStateReader>();
            builder.RegisterType<ApplicationStateWriter>();

            builder.Register(ApplicationStateReader)
                .AsImplementedInterfaces();
            builder.Register(ApplicationStateWriter)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<JsonConverter>()
                .As<JsonConverter>();
            builder.Register(JsonSerializerOptions)
                .SingleInstance();
        }

        private Encoding TimetableFileEncoding(IComponentContext context)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding(1250);
        }

        private Task<Timetable.Timetable> ReadTimetable(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            var reader = context.Resolve<IReader>(parameters);
            var timetableReader = context.Resolve<ITimetableReader>(TypedParameter.From(reader));
            var timetableReadTask = timetableReader.Read();
            
            timetableReadTask.ContinueWith(_ => timetableReader.Dispose());
            return timetableReadTask;
        }

        private CsvReader CsvReader(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            var path = parameters.TypedAs<string>();
            var encoding = context.Resolve<Encoding>();
            var stream = new StreamReader(path, encoding);

            return new CsvReader(stream, CsvConfiguration);
        }

        private CsvWriter CsvWriter(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            var path = parameters.TypedAs<string>();
            var encoding = context.Resolve<Encoding>();
            var stream = new StreamWriter(path, false, encoding);

            return new CsvWriter(stream, CsvConfiguration);
        }

        public static Configuration CsvConfiguration
        {
            get
            {
                var config = new Configuration
                {
                    Delimiter = " ",
                    HasHeaderRecord = false,
                    ShouldSkipRecord = record => record[0] == "info"
                };

                config.RegisterClassMap<TimetableRecord.Map>();
                config.RegisterClassMap<Entry.Map>();

                return config;
            }
        }

        private ApplicationStateReader ApplicationStateReader(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            var path = parameters.TypedAs<string>();
            var stream = new FileStream(path, FileMode.Open);

            return context.Resolve<ApplicationStateReader>(new TypedParameter(typeof(Stream), stream));
        }

        private ApplicationStateWriter ApplicationStateWriter(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            var path = parameters.TypedAs<string>();
            var stream = new FileStream(path, FileMode.Create);

            return context.Resolve<ApplicationStateWriter>(new TypedParameter(typeof(Stream), stream));
        }

        private JsonSerializerOptions JsonSerializerOptions(IComponentContext context)
        {
            var converters = context.Resolve<IEnumerable<JsonConverter>>();
            var options = new JsonSerializerOptions { WriteIndented = true };

            foreach (var converter in converters)
                options.Converters.Add(converter);

            return options;
        }
    }
}
