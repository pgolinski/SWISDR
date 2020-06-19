using System;
using System.Text.RegularExpressions;

namespace SWISDR.Core.CsvConverters
{
    public class CategoryConverter : ConverterBase<string>
    {
        private static Regex Regex = new Regex("([A-Z]+)[a-z]*");
        public override string ConvertFromString(string text)
        {
            var match = Regex.Match(text);
            if (!match.Success)
                throw new FormatException($"Unable to parse category from {text}");

            return match.Groups[1].Value;
        }
    }
}
