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
    public class TimeConverterTests
    {
        [Test]
        public void should_convert_none_literal_to_null()
        {
            var converter = new TimeConverter();
            converter.ConvertFromString(TimetableLiterals.None, Mock.Of<IReaderRow>(), null).Should().BeNull();
        }

        [Test]
        public void should_conver_time_from_string()
        {
            var converter = new TimeConverter();
            var result = (TimeSpan)converter.ConvertFromString("10:15", Mock.Of<IReaderRow>(), null);
            result.Should().Be(new TimeSpan(10, 15, 0));
        }
    }
}