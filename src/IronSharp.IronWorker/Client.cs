using System.Threading;
using IronSharp.Core;

namespace IronSharp.IronWorker
{
    /// <summary>
    /// http://dev.iron.io/worker/reference/api/#list_code_packages
    /// </summary>
    public class Client
    {
        private readonly IronClientConfig _config;

        private Client(IronClientConfig config)
        {
            _config = LazyInitializer.EnsureInitialized(ref config);

            if (string.IsNullOrEmpty(Config.Host))
            {
                Config.Host = CloudHosts.DEFAULT;
            }

            if (config.Version == default (int))
            {
                config.Version = 2;
            }
        }

        public IronClientConfig Config
        {
            get { return _config; }
        }

        public string EndPoint
        {
            get { return "/projects/{Project ID}"; }
        }

        public static Client @New(string projectId = null, string token = null, string host = null)
        {
            return New(new IronClientConfig
            {
                Host = host,
                ProjectId = projectId,
                Token = token
            });
        }

        public static Client @New(IronClientConfig config = null)
        {
            return new Client(config);
        }

        #region Code

        public CodeClient Code(string codeId)
        {
            return new CodeClient(this, codeId);
        }

        public CodeInfoCollection Codes(int? page = null, int? perPage = null)
        {
            return Codes(new PagingFilter
            {
                Page = page.GetValueOrDefault(),
                PerPage = perPage.GetValueOrDefault()
            });
        }

        /// <summary>
        /// List code packages
        /// </summary>
        /// <param name="filter"> </param>
        /// <remarks>
        /// http://dev.iron.io/worker/reference/api/#list_code_packages
        /// </remarks>
        public CodeInfoCollection Codes(PagingFilter filter = null)
        {
            return RestClient.Get<CodeInfoCollection>(_config, string.Format("{0}/codes", EndPoint), filter).Result;
        }

        #endregion

        #region Task

        public TaskClient Tasks
        {
            get { return new TaskClient(this); }
        }

        #endregion

        #region Schedule

        public ScheduleClient Schedules
        {
            get { return new ScheduleClient(this); }
        }

        #endregion
    }
}