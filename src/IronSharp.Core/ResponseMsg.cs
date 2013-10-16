using Newtonsoft.Json;

namespace IronSharp.Core
{
    public class ResponseMsg
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
    }
}