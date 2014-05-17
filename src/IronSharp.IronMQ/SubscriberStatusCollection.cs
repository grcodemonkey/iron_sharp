using System.Collections.Generic;
using System.Threading;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronMQ
{
    public class SubscriberStatusCollection : IInspectable
    {
        [JsonProperty("subscribers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private List<SubscriberStatus> _subscribers;

        [JsonIgnore]
        public List<SubscriberStatus> Subscribers
        {
            get { return LazyInitializer.EnsureInitialized(ref _subscribers); }
            set { _subscribers = value; }
        }
    }
}