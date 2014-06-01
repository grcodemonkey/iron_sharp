namespace IronSharp.Core
{
    /// <summary>
    ///     Custom Http Headers present in message from a push queue.
    /// </summary>
    public static class IronHeaders
    {
        public const string IRON_SUBSCRIBER_MESSAGE_ID = "Iron-Subscriber-Message-Id";

        public const string X_REQUEST_ID = "X-Request-Id";

        public const string IRON_SUBSCRIBER_MESSAGE_URL = "Iron-Subscriber-Message-Url";

        public const string IRON_MESSAGE_ID = "Iron-Message-Id";
    }
}