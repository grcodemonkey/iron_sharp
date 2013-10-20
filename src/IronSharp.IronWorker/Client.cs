using IronSharp.Core;

namespace IronSharp.IronWorker
{
    /// <summary>
    /// http://dev.iron.io/worker/reference/api/#list_code_packages
    /// </summary>
    public static class Client
    {
        public static IronWorkerRestClient @New()
        {
            return New(null);
        }

        public static IronWorkerRestClient @New(string projectId, string token = null, string host = null)
        {
            return New(new IronClientConfig
            {
                Host = host,
                ProjectId = projectId,
                Token = token
            });
        }

        public static IronWorkerRestClient @New(IronClientConfig config)
        {
            IronClientConfig hierarchyConfig = IronDotConfigManager.Load(IronProduct.IronWorker, config);

            return new IronWorkerRestClient(hierarchyConfig);
        }
    }
}