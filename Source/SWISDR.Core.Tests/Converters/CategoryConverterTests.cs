using CsvHelper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SWISDR.Core.CsvConverters;

namespace SWISDR.Core.Tests.Converters
{
    [TestFixture]
    public class CategoryConverterTests
    {
        [TestCase("TMEa", "TME")]
        [TestCase("MPSi", "MPS")]
        [TestCase("APMr", "APM")]
        public void should_convert_from_string(string input, string expected)
        {
            var converter = new CategoryConverter();
            var result = converter.ConvertFromString(input, Mock.Of<IReaderRow>(), null);
            result.Should().Be(expected);
        }
    }
}