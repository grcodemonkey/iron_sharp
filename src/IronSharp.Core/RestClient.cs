using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace IronSharp.Core
{
    public class RestClient
    {
        public static HttpClient Create(Uri baseAddress, string authenticationToken)
        {
            return new HttpClient { BaseAddress = baseAddress };
        }

        public static RestResponse<T> Delete<T>(IronClientConfig config, string endPoint, NameValueCollection query = null, Object payload = null) where T : class 
        {
            HttpRequestMessage request = BuildRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Delete
            });

            IronSharpConfig sharpConfig = config.SharpConfig;

            if (payload != null)
            {
                request.Content = new JsonContent(sharpConfig.ValueSerializer, payload);
            }

            return new RestResponse<T>(AttemptRequest(sharpConfig, request));
        }

        public static Task<HttpResponseMessage> Execute(IronClientConfig config, IRestClientRequest request)
        {
            HttpRequestMessage httpRequest = BuildRequest(config, new RestClientRequest
            {
                EndPoint = request.EndPoint,
                Query = request.Query,
                Method = request.Method,
                Content = request.Content
            });

            using (var client = new HttpClient())
            {
                return client.SendAsync(httpRequest);
            }
        }

        public static RestResponse<T> Get<T>(IronClientConfig config, string endPoint, NameValueCollection query = null) where T : class 
        {
            HttpRequestMessage request = BuildRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Get
            });

            return new RestResponse<T>(AttemptRequest(config.SharpConfig, request));
        }

        public static RestResponse<T> Post<T>(IronClientConfig config, string endPoint, object payload = null, NameValueCollection query = null) where T : class 
        {
            HttpRequestMessage request = BuildRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Post
            });

            IronSharpConfig sharpConfig = config.SharpConfig;

            if (payload != null)
            {
                request.Content = new JsonContent(sharpConfig.ValueSerializer, payload);
            }

            using (var sw = new StringWriter())
            {
                sw.WriteLine("Request: {0}", request.RequestUri);
                if (request.Content != null)
                {
                    sw.WriteLine(request.Content.ReadAsStringAsync().Result);
                }
                File.AppendAllText("log.txt", sw.ToString());
            }

            return new RestResponse<T>(AttemptRequest(sharpConfig, request));
        }

        public static RestResponse<T> Put<T>(IronClientConfig config, string endPoint, object payload, NameValueCollection query = null) where T : class 
        {
            HttpRequestMessage request = BuildRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Put
            });

            IronSharpConfig sharpConfig = config.SharpConfig;

            if (payload != null)
            {
                request.Content = new JsonContent(sharpConfig.ValueSerializer, payload);
            }

            return new RestResponse<T>(AttemptRequest(sharpConfig, request));
        }

        private static HttpResponseMessage AttemptRequest(IronSharpConfig sharpConfig, HttpRequestMessage request, int attempt = 0)
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

                    ExponentialBackoff.Sleep(sharpConfig.BackoffFactor, attempt);

                    return AttemptRequest(sharpConfig, request, attempt);
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