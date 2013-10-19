using System;

namespace IronSharp.IronWorker
{
    public static class ScheduleBuilder
    {
        public static readonly TimeSpan Month = TimeSpan.FromDays(30.4368);

        public static ScheduleOptionsBuilder Build(DateTime? now = null)
        {
            return new ScheduleOptionsBuilder(now.GetValueOrDefault(DateTime.Now));
        }
    }
}