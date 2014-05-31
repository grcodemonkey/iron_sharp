using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace IronSharp.Core
{
    public static class RestUtility
    {
        public static HttpRequestMessage BuildIronRequest(IronClientConfig config, IRestClientRequest request)
        {
            SetOathQueryParameterIfRequired(request, config.Token);
            var httpRequest = new HttpRequestMessage
            {
                Content = request.Content,
                RequestUri = BuildProjectUri(config, request.EndPoint, request.Query),
                Method = request.Method
            };

            HttpRequestHeaders headers = httpRequest.Headers;
            SetOauthHeaderIfRequired(config, request, headers);
            headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue(request.Accept));

            return httpRequest;
        }

        public static Uri BuildProjectUri(IronClientConfig config, string path, NameValueCollection query)
        {
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            string queryString = BuildQueryString(query);

            var uriBuilder = new UriBuilder(Uri.UriSchemeHttps, config.Host)
            {
                Path = String.Format("{0}/{1}", config.ApiVersion, path.Replace("{Project ID}", config.ProjectId)),
                Query = queryString
            };

            return uriBuilder.Uri;
        }

        public static string BuildQueryString(NameValueCollection query)
        {
            string queryString = "";
            if (query != null && query.Count > 0)
            {
                NameValueCollection nameValueCollection = HttpUtility.ParseQueryString("");
                nameValueCollection.Add(query);
                queryString = nameValueCollection.ToString();
            }
            return queryString;
        }

        public static bool IsRetriableStatusCode(HttpResponseMessage response)
        {
            return response != null && response.StatusCode == HttpStatusCode.ServiceUnavailable;
        }

        public static void SetOathQueryParameterIfRequired(IRestClientRequest request, string token)
        {
            if (request.AuthTokenLocation != AuthTokenLocation.Querystring) return;

            request.Query = request.Query ?? new NameValueCollection();
            request.Query["oauth"] = token;
        }

        public static void SetOauthHeaderIfRequired(IronClientConfig config, IRestClientRequest request, HttpRequestHeaders headers)
        {
            if (request.AuthTokenLocation == AuthTokenLocation.Header)
            {
                headers.Authorization = new AuthenticationHeaderValue("OAuth", config.Token);
            }
        }
    }
}