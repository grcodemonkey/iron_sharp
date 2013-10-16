using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace IronSharp.IronMQ
{
    public class MessageIdCollection
    {
        private List<string> _ids;

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
    }
}