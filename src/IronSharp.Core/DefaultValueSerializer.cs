using System;
using Newtonsoft.Json;

namespace IronSharp.Core
{
    public class DefaultValueSerializer : IValueSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public DefaultValueSerializer() : this(null)
        {
        }

        public DefaultValueSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public string Generate(object value)
        {
            return JSON.Generate(value, _settings);
        }

        public T Parse<T>(string value)
        {
            return JSON.Parse<T>(value, _settings);
        }
    }
}