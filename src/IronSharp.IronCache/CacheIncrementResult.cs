using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronCache
{
    public class CacheIncrementResult : IMsg, IInspectable
    {
        [JsonProperty("msg")]
        public string Message { get; set; }

        public bool Success
        {
            get { return this.HasExpectedMessage("Added"); }
        }

        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Value { get; set; }

        public static implicit operator bool(CacheIncrementResult value)
        {
            return value != null && value.Success;
        }
    }
}