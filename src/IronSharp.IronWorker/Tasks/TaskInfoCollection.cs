using System.Collections.Generic;
using System.Threading;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronWorker
{
    public class TaskInfoCollection : IInspectable
    {
        private List<TaskInfo> _tasks;

        [JsonProperty("tasks")]
        public List<TaskInfo> Tasks
        {
            get { return LazyInitializer.EnsureInitialized(ref _tasks); }
            set { _tasks = value; }
        }
    }
}