using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using IronSharp.Core;

namespace IronSharp.IronCache
{
    public class IronCacheRestClient
    {
        private readonly IronClientConfig _config;

        internal IronCacheRestClient(IronClientConfig config)
        {
            _config = LazyInitializer.EnsureInitialized(ref config);

            if (string.IsNullOrEmpty(Config.Host))
            {
                Config.Host = IronCacheCloudHosts.DEFAULT;
            }

            Config.ApiVersion = config.ApiVersion.GetValueOrDefault(1);
        }

        public IronClientConfig Config
        {
            get { return _config; }
        }

        public string EndPoint
        {
            get { return "/projects/{Project ID}/caches"; }
        }


        public CacheClient Cache(string cacheName)
        {
            return new CacheClient(this, cacheName);
        }

        /// <summary>
        /// Delete a cache and all items in it.
        /// </summary>
        /// <param name="cacheName"> The name of the cache </param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#delete_a_cache
        /// </remarks>
        public async Task<bool> Delete(string cacheName)
        {
            return await RestClient.Delete<ResponseMsg>(_config, string.Format("{0}/{1}", EndPoint, cacheName)).HasExpectedMessage("Deleted.");
        }

        /// <summary>
        /// Get a list of all caches in a project. 100 caches are listed at a time. To see more, use the page parameter.
        /// </summary>
        /// <param name="page"> The current page </param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#list_caches
        /// </remarks>
        public async Task<CacheInfo[]> List(int? page)
        {
            var query = new NameValueCollection();

            if (page.HasValue)
            {
                query.Add("page", Convert.ToString(page));
            }

            return await RestClient.Get<CacheInfo[]>(_config, EndPoint, query);
        }
    }
}