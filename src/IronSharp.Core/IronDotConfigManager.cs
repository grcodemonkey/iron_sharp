using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace IronSharp.Core
{
    public static class IronDotConfigManager
    {
        public static string GetEnvironmentKey(IronProduct product, string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            string productName = GetProductName(product);

            return string.Format("{0}_{1}", productName, key).ToUpper();
        }

        public static IronClientConfig Load(IronProduct product = IronProduct.AllProducts, IronClientConfig overrideConfig = null)
        {
            string homeIronDotJson = Path.Combine(GetHomeDirectory(), ".iron.json");
            string appIronDotJson = Path.Combine(GetCurrentDirectory(), "iron.json");

            IronClientConfig home = ParseJsonFile(product, homeIronDotJson);
            IronClientConfig app = ParseJsonFile(product, appIronDotJson);

            ApplyOverrides(home, app);
            ApplyOverrides(home, overrideConfig);

            return home;
        }

        public static IronClientConfig ParseJsonFile(IronProduct product, string filePath)
        {
            JsonDotConfigModel config = LoadJson(filePath);

            var settings = new IronClientConfig
            {
                ProjectId = config.ProjectId,
                Token = config.Token,
                Host = config.Host
            };

            ApplyOverrides(settings, GetProductOverride(product, config));

            return settings;
        }

        private static IronClientConfig GetProductOverride(IronProduct product, JsonDotConfigModel config)
        {
            IronClientConfig productOverride = null;

            switch (product)
            {
                case IronProduct.IronCache:
                    productOverride = config.IronCache;
                    break;
                case IronProduct.IronMQ:
                    productOverride = config.IronMQ;
                    break;
                case IronProduct.IronWorker:
                    productOverride = config.IronWoker;
                    break;
            }
            return productOverride;
        }

        private static void ApplyOverrides(IronClientConfig targetConfig, IronClientConfig overrideConfig)
        {
            if (overrideConfig == null)
            {
                return;
            }

            targetConfig.ProjectId = string.IsNullOrEmpty(overrideConfig.ProjectId) ? targetConfig.ProjectId : overrideConfig.ProjectId;
            targetConfig.Token = string.IsNullOrEmpty(overrideConfig.Token) ? targetConfig.Token : overrideConfig.Token;
            targetConfig.Host = string.IsNullOrEmpty(overrideConfig.Host) ? targetConfig.Host : overrideConfig.Host;
        }

        private static string GetCurrentDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
        }

        private static string GetHomeDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        private static string GetProductName(IronProduct product)
        {
            string productName;

            switch (product)
            {
                case IronProduct.AllProducts:
                    productName = "iron";
                    break;
                case IronProduct.IronMQ:
                    productName = "iron_mq";
                    break;
                case IronProduct.IronWorker:
                    productName = "iron_worker";
                    break;
                case IronProduct.IronCache:
                    productName = "iron_cache";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("product");
            }
            return productName;
        }

        private static JsonDotConfigModel LoadJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<JsonDotConfigModel>(File.ReadAllText(filePath));
            }
            return new JsonDotConfigModel();
        }

        private class JsonDotConfigModel : IronClientConfig
        {
            [JsonProperty("iron_cache")]
            public IronClientConfig IronCache { get; set; }

            [JsonProperty("iron_mq")]
            public IronClientConfig IronMQ { get; set; }

            [JsonProperty("iron_worker")]
            public IronClientConfig IronWoker { get; set; }
        }
    }
}