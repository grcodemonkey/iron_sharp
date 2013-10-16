using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace IronSharp.Core
{
    public class JsonContent : StringContent
    {
        public JsonContent(Object content)
            : base(JsonConvert.SerializeObject(content))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}