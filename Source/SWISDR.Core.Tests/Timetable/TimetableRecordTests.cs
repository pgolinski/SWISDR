using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using NUnit.Framework;
using SWISDR.Core.Timetable;
using System;
using System.IO;
using System.Text;

namespace SWISDR.Core.Tests.Timetable
{
    [TestFixture]
    public class TimetableRecordTests
    {
        [Test]
        public void should_be_parsed_from_passing_through_train_data_row()
        {
            var row = "1,2 0 1-7 00:26 none 100 2 10 5 50 843011 2 none none 0 M62,4,1,wagony[vm=70],85,0 none none 01:15 none none none none none bk wi none S5 -1 TMEa 00:26 00:32 none 01:30 Zbyszki Wilamowice D none 0 1 none zmiana_lokomotywy_w_Liskowie Stałego_kursowania Roczny 0 0 0 none none";

            var record = ParseRecord(row);

            record.RunDays.Should().NotBeNull();
            record.Number.Should().Be(843011);
            record.Category.Should().Be("TME");
            record.From.Should().Be("BK");
            record.To.Should().Be("WI");
            record.AdjacentDeparture.Should().Be(new TimeSpan(0, 26, 0));
            record.Arrival.Should().Be(new TimeSpan(0, 32, 0));
            record.Departure.Should().Be(new TimeSpan(1, 15, 0));
            record.AdjacentArrival.Should().Be(new TimeSpan(1, 30, 0));
            record.Notes.Should().Be("zmiana lokomotywy w Liskowie");

        }

        [Test]
        public void should_be_parsed_from_starting_train_data_row()
        {
            var row = "none 0 1-5 none none 100 2 10 5 120 960 2 none none 0 none none none 02:51 none none none none none ls tm none none 0 TKSc none none none 02:57 Lisków TEFAMA none none 0 1 none zabiera_wagony_od_66180 Stałego_kursowania Roczny 0 0 0 none none";

            var record = ParseRecord(row);

            record.RunDays.Should().NotBeNull();
            record.Number.Should().Be(960);
            record.Category.Should().Be("TKS");
            record.From.Should().Be("LS");
            record.To.Should().Be("TM");
            record.AdjacentDeparture.Should().BeNull();
            record.Arrival.Should().BeNull();
            record.Departure.Should().Be(new TimeSpan(2, 51, 0));
            record.AdjacentArrival.Should().Be(new TimeSpan(2, 57, 0));
            record.Notes.Should().Be("zabiera wagony od 66180");
        }

        [Test]
        public void should_be_parsed_with_track_info()
        {
            var row = "1,2 0 1-7 01:28 none 100 2 10 5 120 63207 5 none none 0 SU45,4,1,wagony_p.,45,0 1 R,S,Z11,Z13,Z15 01:50 none none none none none bk wi none none 255 MPSi 01:28 01:33 1 02:01 Zbyszki Wilamowice none none 1 1 none zamiana_lokomotywy_z_poc_36204 Stałego_kursowania Roczny 0 0 0 none none";

            var record = ParseRecord(row);
            record.Should().NotBeNull();
        }

        private TimetableRecord ParseRecord(string row)
        {
            using var stream = new MemoryStream();
            stream.Write(Encoding.UTF8.GetBytes(row));
            stream.Seek(0, SeekOrigin.Begin);

            var config = new Configuration
            {
                HasHeaderRecord = false,
                Delimiter = " "
            };

            config.RegisterClassMap<TimetableRecord.Map>();
            using var csvReader = new CsvReader(new StreamReader(stream), config);

            csvReader.Read();

            return csvReader.GetRecord<TimetableRecord>();
        }
    }
}
