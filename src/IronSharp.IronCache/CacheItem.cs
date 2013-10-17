using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronCache
{
    public class CacheItem : CacheItemOptions
    {
        public CacheItem()
        {
        }

        public CacheItem(object value, CacheItemOptions options = null, JsonSerializerSettings settings = null)
            : this(JSON.Generate(value, settings), options)
        {
        }

        public CacheItem(string value, CacheItemOptions options = null)
        {
            Value = value;

            if (options == null) return;

            ExpiresIn = options.ExpiresIn;
            Replace = options.Replace;
            Add = options.Add;
            Cas = options.Cas;
        }

        [JsonProperty("cache", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Cache { get; set; }

        [JsonProperty("key", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public T ReadValueAs<T>(JsonSerializerSettings settings = null)
        {
            return JSON.Parse<T>(Value, settings);
        }
    }
}