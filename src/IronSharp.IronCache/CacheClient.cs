using System;
using System.Collections.Specialized;
using IronSharp.Core;
using Newtonsoft.Json;

namespace IronSharp.IronCache
{
    public class CacheClient
    {
        private readonly Client _client;

        public CacheClient(Client client)
        {
            _client = client;
        }

        /// <summary>
        /// Delete all items in a cache. This cannot be undone.
        /// </summary>
        /// <param name="cacheName">The name of the cache whose items should be cleared.</param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#clear_a_cache
        /// </remarks>
        public bool Clear(string cacheName)
        {
            return RestClient.Post<ResponseMsg>(_client.Config, CacheNameEndPoint(cacheName) + "/clear").HasExpectedMessage("Deleted.");
        }

        /// <summary>
        /// Delete a cache and all items in it.
        /// </summary>
        /// <param name="cacheName">The name of the cache</param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#delete_a_cache
        /// </remarks>
        public bool Delete(string cacheName)
        {
            return RestClient.Delete<ResponseMsg>(_client.Config, CacheNameEndPoint(cacheName)).HasExpectedMessage("Deleted.");
        }

        public bool Delete(string cacheName, string key)
        {
            return RestClient.Delete<ResponseMsg>(_client.Config, CacheItemEndPoint(cacheName, key)).HasExpectedMessage("Deleted.");
        }

        /// <summary>
        /// This call gets general information about a cache.
        /// </summary>
        /// <param name="cacheName">The name of the cache</param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#get_info_about_a_cache
        /// </remarks>
        public dynamic Get(string cacheName)
        {
            return RestClient.Get<dynamic>(_client.Config, CacheNameEndPoint(cacheName));
        }

        /// <summary>
        /// This call retrieves an item from the cache. The item will not be deleted.
        /// </summary>
        /// <param name="cacheName">The name of the cache the item belongs to.</param>
        /// <param name="key">The key the item is stored under in the cache.</param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#get_an_item_from_a_cache
        /// </remarks>
        public CacheItem Get(string cacheName, string key)
        {
            return RestClient.Get<CacheItem>(_client.Config, CacheItemEndPoint(cacheName, key));
        }

        public T Get<T>(string cacheName, string key, JsonSerializerSettings settings = null)
        {
            CacheItem item = Get(cacheName, key);

            if (item == null || string.IsNullOrEmpty(item.Value))
            {
                return default(T);
            }

            return item.ReadValueAs<T>(settings);
        }

        public T GetOrAdd<T>(string cacheName, string key, Func<T> valueFactory, CacheItemOptions options = null, JsonSerializerSettings settings = null)
        {
            var item = Get<T>(cacheName, key, settings);

            if (Equals(item, default(T)))
            {
                item = valueFactory();
                Put(cacheName, key, item, options, settings);
            }

            return item;
        }

        public CacheItem GetOrAdd(string cacheName, string key, Func<CacheItem> valueFactory)
        {
            CacheItem item = Get(cacheName, key);

            if (item == null || string.IsNullOrEmpty(item.Value))
            {
                item = valueFactory();
                Put(cacheName, key, item);
            }

            return item;
        }

        /// <summary>
        /// This call increments the numeric value of an item in the cache. The amount must be a number and attempting to increment non-numeric values results in an error.
        /// Negative amounts may be passed to decrement the value.
        /// The increment is atomic, so concurrent increments will all be observed.
        /// </summary>
        /// <param name="cacheName"> The name of the cache. If the cache does not exist, it will be created for you. </param>
        /// <param name="key"> The key of the item to increment </param>
        /// <param name="amount"> The amount to increment the value, as an integer. If negative, the value will be decremented. </param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#increment_an_items_value
        /// </remarks>
        public CacheIncrementResult Increment(string cacheName, string key, int amount)
        {
            return RestClient.Post<CacheIncrementResult>(_client.Config, string.Format("{0}/increment", CacheItemEndPoint(cacheName, key)), new { amount });
        }

        /// <summary>
        /// Get a list of all caches in a project. 100 caches are listed at a time. To see more, use the page parameter.
        /// </summary>
        /// <param name="page">The current page</param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#list_caches
        /// </remarks>
        public CacheInfo[] List(int? page)
        {
            var query = new NameValueCollection();

            if (page.HasValue)
            {
                query.Add("page", Convert.ToString(page));
            }

            return RestClient.Get<CacheInfo[]>(_client.Config, _client.EndPoint, query);
        }

        public bool Put(string cacheName, string key, object value, CacheItemOptions options = null, JsonSerializerSettings settings = null)
        {
            return Put(cacheName, key, new CacheItem(value, options, settings));
        }

        public bool Put(string cacheName, string key, string value, CacheItemOptions options = null)
        {
            return Put(cacheName, key, new CacheItem(value, options));
        }

        /// <summary>
        /// This call puts an item into a cache.
        /// </summary>
        /// <param name="cacheName">The name of the cache. If the cache does not exist, it will be created for you.</param>
        /// <param name="key">The key to store the item under in the cache.</param>
        /// <param name="item">The item’s data</param>
        /// <remarks>
        /// http://dev.iron.io/cache/reference/api/#put_an_item_into_a_cache
        /// </remarks>
        public bool Put(string cacheName, string key, CacheItem item)
        {
            return RestClient.Put<ResponseMsg>(_client.Config, CacheItemEndPoint(cacheName, key), item).HasExpectedMessage("Stored.");
        }

        private string CacheItemEndPoint(string cacheName, string key)
        {
            return string.Format("{0}/items/{1}", CacheNameEndPoint(cacheName), key);
        }

        private string CacheNameEndPoint(string cacheName)
        {
            return string.Format("{0}/{1}", _client.EndPoint, cacheName);
        }
    }
}