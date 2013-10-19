using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IronSharp.Core
{
    public class JsonContent : StringContent
    {
        public JsonContent(IValueSerializer valueSerializer, Object content)
            : base(valueSerializer.Generate(content))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}