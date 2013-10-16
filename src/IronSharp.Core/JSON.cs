using System;
using System.Threading;
using Newtonsoft.Json;

namespace IronSharp.Core
{
    public static class JSON
    {
        private static JsonSerializerSettings _settings;

        public static JsonSerializerSettings Settings
        {
            get { return LazyInitializer.EnsureInitialized(ref _settings, GetDefaultSerializerSettings ); }
            set { _settings = value; }
        }

        private static JsonSerializerSettings GetDefaultSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
        }

        public static string Generate(object value, JsonSerializerSettings opts = null)
        {
            return JsonConvert.SerializeObject(value, opts ?? Settings);
        }

        public static T Parse<T>(string value, JsonSerializerSettings opts = null)
        {
            return JsonConvert.DeserializeObject<T>(value, opts ?? Settings);
        }
    }
}