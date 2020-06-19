using CsvHelper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SWISDR.Core.CsvConverters;
using SWISDR.Core.Timetable;
using System;

namespace SWISDR.Core.Tests.Converters
{
    [TestFixture]
    public class RunDaysConverterTests
    {
        [Test]
        public void should_convert_from_string()
        {
            var converter = new RunDaysConverter();
            var runDays = (RunDays)converter.ConvertFromString("6-7", Mock.Of<IReaderRow>(), null);

            runDays.RunsThisDay(DayOfWeek.Monday).Should().BeFalse();
            runDays.RunsThisDay(DayOfWeek.Friday).Should().BeFalse();
            runDays.RunsThisDay(DayOfWeek.Saturday).Should().BeTrue();
            runDays.RunsThisDay(DayOfWeek.Sunday).Should().BeTrue();
        }
    }
}