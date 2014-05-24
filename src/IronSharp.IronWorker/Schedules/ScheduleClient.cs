using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using IronSharp.Core;

namespace IronSharp.IronWorker
{
    public class ScheduleClient
    {
        private readonly IronWorkerRestClient _client;

        public ScheduleClient(IronWorkerRestClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            Contract.EndContractBlock();

            _client = client;
        }

        public string EndPoint
        {
            get { return string.Format("{0}/schedules", _client.EndPoint); }
        }

        public IValueSerializer ValueSerializer
        {
            get { return _client.Config.SharpConfig.ValueSerializer; }
        }

        public async Task<bool> Cancel(string scheduleId)
        {
            return await RestClient.Post<ResponseMsg>(_client.Config, ScheduleEndPoint(scheduleId) + "/cancel").HasExpectedMessage("Cancelled");
        }

        public async Task<ScheduleIdCollection> Create(string codeName, object payload, ScheduleOptions options)
        {
            return await Create(codeName, ValueSerializer.Generate(payload), options);
        }

        public async Task<ScheduleIdCollection> Create(string codeName, string payload, ScheduleOptions options)
        {
            return await Create(new SchedulePayloadCollection(codeName, payload, options));
        }

        public async Task<ScheduleIdCollection> Create(SchedulePayloadCollection collection)
        {
            return await RestClient.Post<ScheduleIdCollection>(_client.Config, EndPoint, collection);
        }

        public async Task<ScheduleInfo> Get(string scheduleId)
        {
            return await RestClient.Get<ScheduleInfo>(_client.Config, ScheduleEndPoint(scheduleId));
        }

        /// <summary>
        ///     List Scheduled Tasks
        /// </summary>
        /// <param name="filter"> </param>
        /// <remarks>
        ///     http://dev.iron.io/worker/reference/api/#list_scheduled_tasks
        /// </remarks>
        public async Task<ScheduleInfoCollection> List(PagingFilter filter = null)
        {
            return await RestClient.Get<ScheduleInfoCollection>(_client.Config, EndPoint, filter);
        }

        public string ScheduleEndPoint(string scheduleId)
        {
            return string.Format("{0}/{1}", EndPoint, scheduleId);
        }
    }
}