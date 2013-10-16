using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace IronSharp.Core
{
    public class RestClient
    {
        public static HttpClient Create(Uri baseAddress, string authenticationToken)
        {
            return new HttpClient { BaseAddress = baseAddress };
        }

        public static RestResponse<T> Delete<T>(IronClientConfig config, string endPoint, NameValueCollection query = null, Object payload = null)
        {
            HttpRequestMessage request = BuildRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Delete
            });

            if (payload != null)
            {
                request.Content = new JsonContent(payload);
            }

            return new RestResponse<T>(AttemptRequest(request));
        }

        public static RestResponse<T> Get<T>(IronClientConfig config, string endPoint, NameValueCollection query = null)
        {
            HttpRequestMessage request = BuildRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Get
            });

            return new RestResponse<T>(AttemptRequest(request));
        }

        public static RestResponse<T> Post<T>(IronClientConfig config, string endPoint, object payload = null, NameValueCollection query = null)
        {
            HttpRequestMessage request = BuildRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Post
            });

            if (payload != null)
            {
                request.Content = new JsonContent(payload);
            }

            return new RestResponse<T>(AttemptRequest(request));
        }

        public static RestResponse<T> Put<T>(IronClientConfig config, string endPoint, object payload, NameValueCollection query = null)
        {
            HttpRequestMessage request = BuildRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Put
            });

            if (payload != null)
            {
                request.Content = new JsonContent(payload);
            }

            return new RestResponse<T>(AttemptRequest(request));
        }

        private static HttpResponseMessage AttemptRequest(HttpRequestMessage request, int attempt = 0)
        {
            if (attempt > HttpClientOptions.RetryLimit)
            {
                throw new MaximumRetryAttemptsExceededException(request, HttpClientOptions.RetryLimit);
            }

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                if (HttpClientOptions.EnableRetry && IsRetriableStatusCode(response))
                {
                    attempt++;

                    ExponentialBackoff.Sleep(attempt);

                    return AttemptRequest(request, attempt);
                }

                return response;
            }
        }

        private static HttpRequestMessage BuildRequest(IronClientConfig config, IRestClientRequest request)
        {
            var httpRequest = new HttpRequestMessage
            {
                Content = request.Content,
                RequestUri = BuildUri(config, request.EndPoint, request.Query),
                Method = request.Method
            };

            HttpRequestHeaders headers = httpRequest.Headers;

            headers.Authorization = new AuthenticationHeaderValue("OAuth", config.Token);
            headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("appliction/json"));

            return httpRequest;
        }

        private static Uri BuildUri(IronClientConfig config, string path, NameValueCollection query)
        {
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            string queryString = "";

            if (query != null && query.Count > 0)
            {
                NameValueCollection httpValueCollection = HttpUtility.ParseQueryString("");

                httpValueCollection.Add(query);

                queryString = httpValueCollection.ToString();
            }

            var uriBuilder = new UriBuilder(Uri.UriSchemeHttps, config.Host)
            {
                Path = string.Format("{0}/{1}", config.Version, path.Replace("{Project ID}", config.ProjectId)),
                Query = queryString
            };

            return uriBuilder.Uri;
        }

        private static bool IsRetriableStatusCode(HttpResponseMessage response)
        {
            return response != null && response.StatusCode == HttpStatusCode.ServiceUnavailable;
        }
    }
}