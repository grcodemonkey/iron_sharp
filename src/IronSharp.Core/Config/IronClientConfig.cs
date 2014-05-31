using System.Threading;
using Newtonsoft.Json;

namespace IronSharp.Core
{
    public class IronClientConfig : RestClientConfig, IIronSharpConfig
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("api_version")]
        public int? ApiVersion { get; set; }

        #region SharpConfig

        private IronSharpConfig _sharpConfig;

        [JsonProperty("sharp_config")]
        public IronSharpConfig SharpConfig
        {
            get { return LazyInitializer.EnsureInitialized(ref _sharpConfig, CreateDefaultIronSharpConfig); }
            set { _sharpConfig = value; }
        }

        private static IronSharpConfig CreateDefaultIronSharpConfig()
        {
            return new IronSharpConfig
            {
                BackoffFactor = 25
            };
        }

        #endregion
    }
}