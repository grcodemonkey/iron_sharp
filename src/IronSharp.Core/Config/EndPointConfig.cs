using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace IronSharp.Core
{
    public class EndPointConfig : RestClientConfig
    {
        private Dictionary<string, string> _headers;

        [JsonProperty("port", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Port { get; set; }

        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _headers, () => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Content-Type", "application/json"}
                });
            }
            set { _headers = value; }
        }
    }
}