using System.Collections.Specialized;
using System.Threading;
using Newtonsoft.Json;

namespace IronSharp.Core
{
    public class IronClientConfig : IIronSharpConfig, IInspectable
    {
        private IronSharpConfig _sharpConfig;

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("sharp_config")]
        public IronSharpConfig SharpConfig
        {
            get { return LazyInitializer.EnsureInitialized(ref _sharpConfig, CreateDefaultIronSharpConfig); }
            set { _sharpConfig = value; }
        }

        public static IronClientConfig Read(NameValueCollection settings)
        {
            return new IronClientConfig
            {
                ProjectId = settings["IronSharp:ProjectId"],
                Token = settings["IronSharp:Token"]
            };
        }

        public static IronClientConfig ReadJson(string ironDotJson)
        {
            return JSON.Parse<IronClientConfig>(ironDotJson);
        }

        private static IronSharpConfig CreateDefaultIronSharpConfig()
        {
            return new IronSharpConfig
            {
                BackoffFactor = 25
            };
        }
    }
}