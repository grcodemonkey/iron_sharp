using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IronSharp.Core;

namespace IronSharp.IronWorker
{
   public class IronWorkerRestClient
    {
        private readonly IronClientConfig _config;

        internal IronWorkerRestClient(IronClientConfig config)
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
