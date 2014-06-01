using System;
using System.Threading.Tasks;
using IronSharp.Core;
using IronSharp.IronCache;

namespace Demo.IronSharpConsole
{
    static internal class IronCachExample
    {
        public async static Task Run()
        {
            // =========================================================
            // Iron.io Cache
            // =========================================================

            IronCacheRestClient ironCacheClient = Client.New();

            // Get a Cache object
            CacheClient cache = ironCacheClient.Cache("my_cache");

            // Put value to cache by key
            await cache.Put("number_item", 42);

            CacheItem item =  await cache.Get("number_item");

            // Get value from cache by key
            Console.WriteLine(item.Value);

            // Get value from cache by key
            Console.WriteLine(await cache.Get<int>("number_item"));

            // Numbers can be incremented
            await cache.Increment("number_item", 10);

            // Immediately delete an item
            await cache.Delete("number_item");

            await cache.Put("complex_item", new { greeting = "Hello", target = "world" });

            CacheItem complexItem = await cache.Get("complex_item");

            // Get value from cache by key
            Console.WriteLine(complexItem.Value);

            await cache.Delete("complex_item");

            await cache.Put("sample_class", new SampleClass {Name = "Sample Class CacheItem"});

            SampleClass sampleClassItem = await cache.Get<SampleClass>("sample_class");

            Console.WriteLine(sampleClassItem.Inspect());

            await cache.Delete("sample_class");
        }
    }
}