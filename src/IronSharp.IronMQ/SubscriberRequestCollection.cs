using System;
using System.Collections.Generic;
using System.Threading;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronMQ
{
    public class SubscriberRequestCollection : IInspectable
    {
        [JsonProperty("subscribers", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        private List<SubscriberItem> _subscribers;

        public SubscriberRequestCollection()
        {
        }

        public SubscriberRequestCollection(Uri subscriber) : this(new[] {subscriber})
        {
        }

        public SubscriberRequestCollection(IEnumerable<Uri> subscribers)
        {
            foreach (Uri subscriber in subscribers)
            {
                Subscribers.Add(subscriber);
            }
        }

        public SubscriberRequestCollection(SubscriberItem subscriber)
            : this(new[] {subscriber})
        {
        }

        public SubscriberRequestCollection(IEnumerable<SubscriberItem> subscribers)
        {
            foreach (var subscriber in subscribers)
            {
                Subscribers.Add(subscriber);
            }
        }


        [JsonIgnore]
        public List<SubscriberItem> Subscribers
        {
            get { return LazyInitializer.EnsureInitialized(ref _subscribers); }
            set { _subscribers = value; }
        }
    }
}