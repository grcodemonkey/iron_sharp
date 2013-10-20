using IronSharp.Core;

namespace IronSharp.IronMQ
{
    public static class Client
    {
        public static IronMqRestClient @New()
        {
            return New(null);
        }

        public static IronMqRestClient @New(string projectId, string token = null, string host = null)
        {
            return New(new IronClientConfig
            {
                Host = host,
                ProjectId = projectId,
                Token = token
            });
        }

        public static IronMqRestClient @New(IronClientConfig config)
        {
            IronClientConfig hierarchyConfig = IronDotConfigManager.Load(IronProduct.IronMQ, config);

            return new IronMqRestClient(hierarchyConfig);
        }
    }
}