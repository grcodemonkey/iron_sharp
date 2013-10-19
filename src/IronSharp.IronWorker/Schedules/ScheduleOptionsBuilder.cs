using System;

namespace IronSharp.IronWorker
{
    public class ScheduleOptionsBuilder : ScheduleOptions
    {
        public ScheduleOptionsBuilder(DateTime now)
        {
            Now = now;
        }

        public DateTime Now { get; set; }

        public ScheduleOptionsBuilder Delay(TimeSpan delay)
        {
            return StartingOn(Now + delay);
        }

        public ScheduleOptionsBuilder EndingOn(DateTime endAt)
        {
            EndAt = endAt;
            return this;
        }

        public ScheduleOptionsBuilder Repeat(int times)
        {
            RunTimes = times;
            return this;
        }

        public ScheduleOptionsBuilder RunFor(TimeSpan duration)
        {
            return EndingOn(Now + duration);
        }
        public ScheduleOptionsBuilder StartingOn(DateTime startAt)
        {
            StartAt = startAt;
            return this;
        }
        public ScheduleOptionsBuilder WithFrequency(TimeSpan frequency)
        {
            RunEvery = frequency.Seconds;
            return this;
        }

        public ScheduleOptionsBuilder WithPriority(TaskPriority priority)
        {
            Priority = priority;
            return this;
        }

        private static int Multiply(double a, double b)
        {
            return Convert.ToInt32(a*b);
        }
    }
}