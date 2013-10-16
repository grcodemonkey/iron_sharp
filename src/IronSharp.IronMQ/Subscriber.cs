using Newtonsoft.Json;

namespace IronSharp.IronMQ
{
    public class Subscriber
    {
        public Subscriber() : this(null)
        {
        }

        public Subscriber(string url)
        {
            Url = url;
        }

        [JsonProperty("url")]
        public string Url { get; set; }

        public static implicit operator Subscriber(string url)
        {
            return new Subscriber(url);
        }
    }
}