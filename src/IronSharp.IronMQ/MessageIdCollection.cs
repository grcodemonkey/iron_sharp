using System.Collections.Generic;
using System.Threading;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronMQ
{
    public class MessageIdCollection : IMsg, IInspectable
    {
        private List<string> _ids;

        public MessageIdCollection()
        {
        }

        public MessageIdCollection(IEnumerable<string> messageIds)
        {
            Ids.AddRange(messageIds);
        }

        [JsonProperty("ids")]
        public List<string> Ids
        {
            get { return LazyInitializer.EnsureInitialized(ref _ids); }
            set { _ids = value; }
        }

        [JsonIgnore]
        public bool Success
        {
            get { return this.HasExpectedMessage("Messages put on queue."); }
        }

        [JsonProperty("msg", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        public static implicit operator bool(MessageIdCollection collection)
        {
            return collection.Success;
        }
    }
}