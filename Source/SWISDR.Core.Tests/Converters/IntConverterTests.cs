using CsvHelper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SWISDR.Core.Timetable;
using SWISDR.Core.CsvConverters;

namespace SWISDR.Core.Tests.Converters
{
    [TestFixture]
    public class IntConverterTests
    {
        [Test]
        public void should_convert_from_string()
        {
            var converter = new IntConverter();
            var result = converter.ConvertFromString("10", Mock.Of<IReaderRow>(), null);
            result.Should().Be(10);
        }

        [Test]
        public void should_convert_none_literal_to_nullable_int()
        {
            var converter = new IntConverter();
            var result = converter.ConvertFromString(TimetableLiterals.None, Mock.Of<IReaderRow>(), null);
            result.Should().BeNull();
        }
    }
}