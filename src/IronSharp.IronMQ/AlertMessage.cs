using System;
using Newtonsoft.Json;

namespace IronSharp.IronMQ
{
    public class AlertMessage
    {
        [JsonProperty("source_queue")]
        public string SourceQueue { get; set; }

        [JsonProperty("queue_size")]
        public int QueueSize { get; set; }

        [JsonProperty("alert_id")]
        public string AlertId { get; set; }

        [JsonProperty("alert_type")]
        public AlertType AlertType { get; set; }

        [JsonProperty("alert_direction")]
        public AlertDirection AlertDirection { get; set; }

        [JsonProperty("alert_trigger")]
        public int AlertTrigger { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}