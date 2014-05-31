using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using IronSharp.Core;

namespace IronSharp.IronMQ
{
    public static class SubscriberItemBuilder
    {
        public static Uri BuildUri(EndPointConfig config, string path, NameValueCollection query)
        {
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            var uriBuilder = new UriBuilder(config.Scheme, config.Host)
            {
                Path = path,
                Query = RestUtility.BuildQueryString(query)
            };
            if (config.Port.HasValue)
            {
                uriBuilder.Port = config.Port.Value;
            }
            return uriBuilder.Uri;
        }

        public static SubscriberItem GetSubscriberInfo(Uri endPointUrl, IDictionary<string, string> headers)
        {
            return GetSubscriberInfo(endPointUrl.ToString(), headers);
        }

        public static SubscriberItem GetSubscriberInfo(string endPointUrl, IDictionary<string, string> headers)
        {
            if (headers == null)
            {
                return new SubscriberItem(endPointUrl);
            }
            return new SubscriberItem
            {
                Headers = new Dictionary<string, string>(headers),
                Url = endPointUrl
            };
        }
    }
}