using Newtonsoft.Json;

namespace IronSharp.Core
{
    public class RestClientConfig : IInspectable
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

    }
}