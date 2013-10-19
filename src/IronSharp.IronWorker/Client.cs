using IronSharp.Core;

namespace IronSharp.IronWorker
{
    /// <summary>
    /// http://dev.iron.io/worker/reference/api/#list_code_packages
    /// </summary>
    public static class Client
    {
        public static IronWorkerRestClient @New(string projectId = null, string token = null, string host = null)
        {
            return New(new IronClientConfig
            {
                Host = host,
                ProjectId = projectId,
                Token = token
            });
        }

        public static IronWorkerRestClient @New(IronClientConfig config = null)
        {
            return new IronWorkerRestClient(config);
        }
    }
}