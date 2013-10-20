using IronSharp.Core;

namespace IronSharp.IronCache
{
    public static class Client
    {
        public static IronCacheRestClient @New()
        {
            return New(null);
        }

        public static IronCacheRestClient @New(string projectId, string token = null, string host = null)
        {
            return New(new IronClientConfig
            {
                Host = host,
                ProjectId = projectId,
                Token = token
            });
        }

        public static IronCacheRestClient @New(IronClientConfig config)
        {
            IronClientConfig hierarchyConfig = IronDotConfigManager.Load(IronProduct.IronCache, config);

            return new IronCacheRestClient(hierarchyConfig);
        }
    }
}