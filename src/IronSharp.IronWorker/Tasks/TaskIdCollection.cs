using System.Collections.Generic;
using System.Threading;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronWorker
{
    public class TaskIdCollection : IMsg, IInspectable
    {
        private List<TaskId> _tasks;

        [JsonProperty("tasks")]
        public List<TaskId> Tasks
        {
            get { return LazyInitializer.EnsureInitialized(ref _tasks); }
            set { _tasks = value; }
        }

        [JsonProperty("msg", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonIgnore]
        public bool Success
        {
            get { return this.HasExpectedMessage("Queued up"); }
        }

        public static implicit operator bool(TaskIdCollection collection)
        {
            return collection.Success;
        }
    }
}