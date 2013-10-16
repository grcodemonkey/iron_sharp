using System.Collections.Generic;
using System.Threading;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronWorker
{
    public class ScheduleInfoCollection : IInspectable
    {
        private List<ScheduleInfo> _schedules;

        [JsonProperty("schedules")]
        public List<ScheduleInfo> Schedules
        {
            get { return LazyInitializer.EnsureInitialized(ref _schedules); }
            set { _schedules = value; }
        }
    }
}