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

        public static SubscriberItem GetSubscriberItem(EndPointConfig endPoint, string path, NameValueCollection query)
        {
            return GetSubscriberItem(BuildUri(endPoint, path, query), endPoint.Headers);
        }

        public static SubscriberItem GetSubscriberItem(Uri endPointUrl, IDictionary<string, string> headers)
        {
            return GetSubscriberItem(endPointUrl.ToString(), headers);
        }

        public static SubscriberItem GetSubscriberItem(string endPointUrl, IDictionary<string, string> headers)
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