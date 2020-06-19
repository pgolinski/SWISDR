using SWISDR.Core.Timetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SWISDR.Core.CsvConverters
{
    public class RunDaysConverter : ConverterBase<RunDays>
    {
        private static Regex RangeRegex = new Regex(@"(\d)-(\d)");

        public override RunDays ConvertFromString(string text)
        {
            var match = RangeRegex.Match(text);

            if (match.Success)
                return new RunDays(ParseRange(text));

            if (int.TryParse(text, out var result))
                return new RunDays(new int[] { result });

            throw new FormatException($"Cannot parse {text} to {nameof(RunDays)}");
        }

        private IEnumerable<int> ParseRange(string text)
        {
            var range = text.Split("-").Select(s => int.Parse(s.Trim())).ToArray();

            for (var i = range[0]; i <= range[1]; i++)
                yield return i;
        }
    }
}
