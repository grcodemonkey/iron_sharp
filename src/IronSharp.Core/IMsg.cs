namespace IronSharp.Core
{
    public interface IMsg
    {
        /// <summary>
        /// Gets the information message
        /// </summary>
        string Message { get; }
    }

    public static class ExtensionsForIMsg
    {
        public static bool HasExpectedMessage(this IMsg msg, string message)
        {
            return msg != null && msg.Message == message;
        }
    }
}