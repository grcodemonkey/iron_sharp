using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IronSharp.IronMQ
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PushType
    {
        [EnumMember(Value = "pull")]
        Pull = 0,

        /// <summary>
        /// Multicast is a routing pattern that will push the messages to all the subscribers. http://dev.iron.io/faq/#MQfaq-11
        /// </summary>
        [EnumMember(Value = "multicast")]
        Multicast,

        /// <summary>
        /// Unicast is a routing pattern that will cycle through the endpoints pushing to one endpoint after another until a success push occurs. http://dev.iron.io/faq/#MQfaq-11
        /// </summary>
        [EnumMember(Value = "unicast")]
        Unicast
    }
}