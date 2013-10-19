using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronMQ
{
    public class QueueMessage : IInspectable
    {
        public QueueMessage(string body)
            : this()
        {
            Body = body;
        }

        protected QueueMessage()
        {
        }

        #region Properties

        /// <summary>
        /// The message data
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// The item will not be available on the queue until this many seconds have passed.
        /// Default is 0 seconds.
        /// Maximum is 604,800 seconds (7 days).
        /// </summary>
        [JsonProperty("delay", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Delay { get; set; }

        /// <summary>
        /// How long in seconds to keep the item on the queue before it is deleted.
        /// Default is 604,800 seconds (7 days).
        /// Maximum is 2,592,000 seconds (30 days).
        /// </summary>
        [JsonProperty("expires_in", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ExpiresIn { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("push_status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PushStatus PushStatus { get; set; }

        [JsonProperty("reserved_count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReservedCount { get; set; }

        /// <summary>
        /// After timeout (in seconds), item will be placed back onto queue.
        /// You must delete the message from the queue to ensure it does not go back onto the queue.
        /// Default is 60 seconds.
        /// Minimum is 30 seconds, and maximum is 86,400 seconds (24 hours).
        /// </summary>
        [JsonProperty("timeout", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Timeout { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This call will delete the message. Be sure you call this after you’re done with a message or it will be placed back on the queue.
        /// </summary>
        public bool Delete()
        {
            return Client.Delete(Id);
        }

        /// <summary>
        /// Releases this message and puts it back on the queue as if the message had timed out.
        /// </summary>
        /// <returns></returns>
        public bool Release(int? delay = null)
        {
            return Client.Release(Id, delay);
        }

        /// <summary>
        /// Extends this message's timeout by the duration specified when the message was created, which is 60 seconds by default.
        /// </summary>
        public bool Touch()
        {
            return Client.Touch(Id);
        }

        #endregion

        [JsonIgnore]
        internal QueueClient Client { get; set; }
        
        public static implicit operator QueueMessage(string message)
        {
            return new QueueMessage(message);
        }

        public T ReadValueAs<T>()
        {
            return Client.ValueSerializer.Parse<T>(Body);
        }
    }
}