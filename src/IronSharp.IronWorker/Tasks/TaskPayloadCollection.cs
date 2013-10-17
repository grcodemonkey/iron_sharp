using System.Collections.Generic;
using System.Threading;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronWorker
{
    public class TaskPayloadCollection : IInspectable
    {
        private List<TaskPayload> _schedules;

        public TaskPayloadCollection(TaskPayload payload)
        {
            Schedules.Add(payload);
        }

        public TaskPayloadCollection(IEnumerable<TaskPayload> payloads)
        {
            Schedules.AddRange(payloads);
        }

        public TaskPayloadCollection(string codeName, string payload, TaskOptions options = null)
        {
            Schedules.Add(new TaskPayload(codeName, payload, options));
        }

        public TaskPayloadCollection(string codeName, IEnumerable<string> payloads, TaskOptions options = null)
        {
            foreach (string payload in payloads)
            {
                Schedules.Add(new TaskPayload(codeName, payload, options));
            }
        }

        public TaskPayloadCollection(string codeName, object payload, TaskOptions options = null, JsonSerializerSettings settings = null)
        {
            Schedules.Add(new TaskPayload(codeName, payload, options, settings));
        }

        public TaskPayloadCollection(string codeName, IEnumerable<object> payloads, TaskOptions options = null, JsonSerializerSettings settings = null)
        {
            foreach (object payload in payloads)
            {
                Schedules.Add(new TaskPayload(codeName, payload, options, settings));
            }
        }
        
        [JsonProperty("schedules")]
        public List<TaskPayload> Schedules
        {
            get { return LazyInitializer.EnsureInitialized(ref _schedules); }
            set { _schedules = value; }
        }
    }
}