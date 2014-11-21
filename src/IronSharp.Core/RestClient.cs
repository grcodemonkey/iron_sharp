using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Logging;

namespace IronSharp.Core
{
    public class RestClient
    {
        /// <summary>
        /// Generates the Uri for the specified request.
        /// </summary>
        /// <param name="config">The project id and other config values</param>
        /// <param name="request">The request endpoint and query parameters</param>
        /// <param name="token">(optional) The token to use for the building the request uri if different than the Token specified in the config.</param>
        public static Uri BuildRequestUri(IronClientConfig config, IRestClientRequest request, string token = null)
        {
            if (string.IsNullOrEmpty(token))
            {
                token = config.Token;
            }
            RestUtility.SetOathQueryParameterIfRequired(request, token);
            return RestUtility.BuildProjectUri(config, request.EndPoint, request.Query);
        }

        public static async Task<RestResponse<T>> Delete<T>(IronClientConfig config, string endPoint, NameValueCollection query = null, Object payload = null) where T : class
        {
            HttpRequestMessage request = RestUtility.BuildIronRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Delete
            });

            IronSharpConfig sharpConfig = config.SharpConfig;

            if (payload != null)
            {
                request.Content = new JsonContent(payload);
            }

            return new RestResponse<T>(await AttemptRequestAync(sharpConfig, request));
        }

        public static Task<HttpResponseMessage> Execute(IronClientConfig config, IRestClientRequest request)
        {
            HttpRequestMessage httpRequest = RestUtility.BuildIronRequest(config, new RestClientRequest
            {
                EndPoint = request.EndPoint,
                Query = request.Query,
                Method = request.Method,
                Content = request.Content
            });

            using (var client = CreateHttpClient())
            {
                return client.SendAsync(httpRequest);
            }
        }

        public static async Task<RestResponse<T>> Get<T>(IronClientConfig config, string endPoint, NameValueCollection query = null) where T : class
        {
            HttpRequestMessage request = RestUtility.BuildIronRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Get
            });

            return new RestResponse<T>(await AttemptRequestAync(config.SharpConfig, request));
        }

        public static async Task<RestResponse<T>> Post<T>(IronClientConfig config, string endPoint, object payload = null, NameValueCollection query = null) where T : class
        {
            HttpRequestMessage request = RestUtility.BuildIronRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Post
            });

            IronSharpConfig sharpConfig = config.SharpConfig;

            if (payload != null)
            {
                request.Content = new JsonContent(payload);
            }

            return new RestResponse<T>(await AttemptRequestAync(sharpConfig, request));
        }

        public static async Task<RestResponse<T>> Put<T>(IronClientConfig config, string endPoint, object payload, NameValueCollection query = null) where T : class
        {
            HttpRequestMessage request = RestUtility.BuildIronRequest(config, new RestClientRequest
            {
                EndPoint = endPoint,
                Query = query,
                Method = HttpMethod.Put
            });

            IronSharpConfig sharpConfig = config.SharpConfig;

            if (payload != null)
            {
                request.Content = new JsonContent(payload);
            }

            return new RestResponse<T>(await AttemptRequestAync(sharpConfig, request));
        }

        public static HttpClient CreateHttpClient()
        {
            return HttpClientFactory.Create(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
        }

        private async static Task<HttpResponseMessage> AttemptRequestAync(IronSharpConfig sharpConfig, HttpRequestMessage request, int attempt = 0)
        {
            if (attempt > HttpClientOptions.RetryLimit)
            {
                throw new MaximumRetryAttemptsExceededException(request, HttpClientOptions.RetryLimit);
            }

            ILog logger = LogManager.GetLogger<RestClient>();

            using (var client = CreateHttpClient())
            {
                if (logger.IsDebugEnabled)
                {
                    using (var sw = new StringWriter())
                    {
                        sw.WriteLine("{0} {1}", request.Method, request.RequestUri);
                        if (request.Content != null)
                        {
                            sw.WriteLine(await request.Content.ReadAsStringAsync());
                        }
                        logger.Debug(sw.ToString());
                    }
                }

                HttpResponseMessage response = await client.SendAsync(request);

                if (logger.IsDebugEnabled)
                {
                    if (response.Content != null)
                    {
                        logger.Debug(await response.Content.ReadAsStringAsync());
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                if (HttpClientOptions.EnableRetry && RestUtility.IsRetriableStatusCode(response))
                {
                    attempt++;

                    return await ExponentialBackoff.Sleep(sharpConfig.BackoffFactor, attempt).
                        ContinueWith(task => AttemptRequestAync(sharpConfig, request, attempt)).
                        Unwrap();
                }

                return response;
            }
        }
    }
}