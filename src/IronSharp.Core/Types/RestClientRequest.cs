using System.Collections.Specialized;
using System.Net.Http;

namespace IronSharp.Core
{
    public class RestClientRequest : IRestClientRequest
    {
        public HttpContent Content { get; set; }

        public string EndPoint { get; set; }

        public NameValueCollection Query { get; set; }

        public HttpMethod Method { get; set; }
    }
}