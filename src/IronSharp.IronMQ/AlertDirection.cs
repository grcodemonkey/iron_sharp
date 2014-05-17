using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IronSharp.IronMQ
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum AlertDirection
    {
        /// <summary>
        /// An "asc" setting will trigger alerts as the queue grows in size.
        /// </summary>
        [EnumMember(Value = "asc")] Asc,
        /// <summary>
        /// A "desc" setting will trigger alerts as the queue decreases in size.
        /// </summary>
        [EnumMember(Value = "desc")] Desc
    }
}