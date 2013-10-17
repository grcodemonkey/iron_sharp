using System.Threading;
using IronSharp.Core;

namespace IronSharp.IronCache
{
    public class Client
    {
        private readonly IronClientConfig _config;

        private Client(IronClientConfig config)
        {
            _config = LazyInitializer.EnsureInitialized(ref config);

            if (string.IsNullOrEmpty(Config.Host))
            {
                Config.Host = CloudHosts.DEFAULT;
            }

            if (config.Version == default (int))
            {
                config.Version = 1;
            }
        }

        public IronClientConfig Config
        {
            get { return _config; }
        }

        public string EndPoint
        {
            get { return "/projects/{Project ID}/caches"; }
        }

        public static Client @New(string projectId = null, string token = null, string host = null)
        {
            return New(new IronClientConfig
            {
                Host = host,
                ProjectId = projectId,
                Token = token
            });
        }

        public static Client @New(IronClientConfig config = null)
        {
            return new Client(config);
        }
    }
}