using IronSharp.Core;

namespace IronSharp.IronMQ
{
    public static class Client
    {
        public static IronMqRestClient @New(string projectId = null, string token = null, string host = null)
        {
            return New(new IronClientConfig
            {
                Host = host,
                ProjectId = projectId,
                Token = token
            });
        }

        public static IronMqRestClient @New(IronClientConfig config = null)
        {
            return new IronMqRestClient(config);
        }
    }
}