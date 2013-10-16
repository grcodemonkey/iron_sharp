using System;
using Newtonsoft.Json;

namespace IronSharp.IronWorker
{
    public class ScheduleOptions : PriorityOption
    {
        [JsonProperty("run_every", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? RunEvery { get; set; }

        [JsonProperty("run_times", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? RunTimes { get; set; }

        [JsonProperty("end_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? EndAt { get; set; }

        [JsonProperty("start_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? StartAt { get; set; }
    }
}