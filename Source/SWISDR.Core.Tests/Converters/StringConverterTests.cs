using CsvHelper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SWISDR.Core.Timetable;
using SWISDR.Core.CsvConverters;

namespace SWISDR.Core.Tests.Converters
{
    [TestFixture]
    public class StringConverterTests
    {
        [Test]
        public void should_convert_none_literal_to_null()
        {
            var converter = new StringConverter();
            converter.ConvertFromString(TimetableLiterals.None, Mock.Of<IReaderRow>(), null).Should().BeNull();
        }

        [Test]
        public void should_not_change_other_text_than_none_literal()
        {
            var converter = new StringConverter();
            converter.ConvertFromString("text_other_than_none", Mock.Of<IReaderRow>(), null).Should().Be("text other than none");
        }
    }
}