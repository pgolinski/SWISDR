using System;
using System.Collections.Generic;
using System.Linq;

namespace SWISDR.Core.Timetable
{
    public class RunDays
    {
        DayOfWeek[] _days;

        public RunDays(IEnumerable<int> dayNumbers)
            => _days = dayNumbers.Select(number => ToDayOfWeek(number)).ToArray();

        public bool RunsThisDay(DayOfWeek dayOfWeek) => _days.Contains(dayOfWeek);

        private DayOfWeek ToDayOfWeek(int number) => (DayOfWeek)(number % 7);
        public bool RunsThisDay(int dayNumber) => RunsThisDay(ToDayOfWeek(dayNumber));
    }
}
