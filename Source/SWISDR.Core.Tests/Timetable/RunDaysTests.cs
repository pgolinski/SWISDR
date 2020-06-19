using FluentAssertions;
using NUnit.Framework;
using SWISDR.Core.Timetable;
using System;

namespace SWISDR.Core.Tests.Timetable
{
    [TestFixture]
    public class RunDaysTests
    {
        [TestCase(1, DayOfWeek.Monday)]
        [TestCase(2, DayOfWeek.Tuesday)]
        [TestCase(3, DayOfWeek.Wednesday)]
        [TestCase(4, DayOfWeek.Thursday)]
        [TestCase(5, DayOfWeek.Friday)]
        [TestCase(6, DayOfWeek.Saturday)]
        [TestCase(7, DayOfWeek.Sunday)]
        public void should_be_constructed_using_integers(int number, DayOfWeek expected)
        {
            var runDays = new RunDays(new[] { number });
            runDays.RunsThisDay(expected).Should().BeTrue();
        }

        [Test]
        public void should_return_false_when_asked_for_day_train_doesnt_run()
        {
            var runDays = new RunDays(new[] { 6, 7 });
            runDays.RunsThisDay(DayOfWeek.Monday).Should().BeFalse();
        }

        [Test]
        public void should_return_true_when_asked_for_day_that_traub_runs()
        {
            var runDays = new RunDays(new[] { 6, 7 });
            runDays.RunsThisDay(DayOfWeek.Saturday).Should().BeTrue();
        }
    }
}