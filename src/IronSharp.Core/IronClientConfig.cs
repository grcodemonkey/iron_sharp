using Newtonsoft.Json;

namespace IronSharp.Core
{
    public class IronClientConfig
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}