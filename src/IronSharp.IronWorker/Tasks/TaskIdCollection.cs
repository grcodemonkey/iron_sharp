using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronWorker
{
    public class TaskIdCollection : IMsg, IInspectable, IIdCollection
    {
        private List<TaskId> _tasks;

        [JsonProperty("msg", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonIgnore]
        public bool Success
        {
            get { return this.HasExpectedMessage("Queued up"); }
        }

        [JsonProperty("tasks")]
        public List<TaskId> Tasks
        {
            get { return LazyInitializer.EnsureInitialized(ref _tasks); }
            set { _tasks = value; }
        }

        public static implicit operator bool(TaskIdCollection collection)
        {
            return collection.Success;
        }

        /// <summary>
        /// Returns a list of IDs
        /// </summary>
        public IEnumerable<string> GetIds()
        {
            return Tasks.Select(x=> x.Id);
        }
    }
}