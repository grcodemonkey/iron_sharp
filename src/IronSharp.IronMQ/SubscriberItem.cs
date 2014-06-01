using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace IronSharp.IronMQ
{
    public class SubscriberItem
    {
        private sealed class SubscriberItemEqualityComparer : IEqualityComparer<SubscriberItem>
        {
            public bool Equals(SubscriberItem x, SubscriberItem y)
            {
                return string.Equals(x.Url, y.Url, StringComparison.Ordinal);
            }

            public int GetHashCode(SubscriberItem obj)
            {
                return obj.Url.GetHashCode();
            }
        }

        private static readonly IEqualityComparer<SubscriberItem> SubscriberItemComparerInstance = new SubscriberItemEqualityComparer();

        public static IEqualityComparer<SubscriberItem> SubscriberItemComparer
        {
            get { return SubscriberItemComparerInstance; }
        }

        public SubscriberItem()
        {
        }

        public SubscriberItem(string url)
        {
            Url = url;
        }

        [JsonProperty("headers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private Dictionary<string, string> _headers;

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> Headers
        {
            get { return LazyInitializer.EnsureInitialized(ref _headers, () => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)); }
            set { _headers = value; }
        }

        public static implicit operator SubscriberItem(string url)
        {
            return new SubscriberItem(url);
        }

        public static implicit operator SubscriberItem(Uri uri)
        {
            return new SubscriberItem(uri.ToString());
        }
    }
}