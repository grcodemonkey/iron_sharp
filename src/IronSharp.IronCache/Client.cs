using System;
using System.Collections.Specialized;
using System.Threading;
using IronSharp.Core;

namespace IronSharp.IronCache
{
    public static class Client
    {
        public static IronCacheRestClient @New(string projectId = null, string token = null, string host = null)
        {
            return New(new IronClientConfig
            {
                Host = host,
                ProjectId = projectId,
                Token = token
            });
        }

        public static IronCacheRestClient @New(IronClientConfig config = null)
        {
            return new IronCacheRestClient(config);
        }
    }
}