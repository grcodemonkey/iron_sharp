using IronSharp.Core;

namespace IronSharp.IronWorker
{
    public class ScheduleClient
    {
        private readonly Client _client;

        public ScheduleClient(Client client)
        {
            _client = client;
        }

        public string EndPoint
        {
            get { return string.Format("{0}/schedules", _client.EndPoint); }
        }

        public string ScheduleEndPoint(string scheduleId)
        {
            return string.Format("{0}/{1}", EndPoint, scheduleId);
        }

        /// <summary>
        /// List Scheduled Tasks
        /// </summary>
        /// <param name="filter"> </param>
        /// <remarks>
        /// http://dev.iron.io/worker/reference/api/#list_scheduled_tasks
        /// </remarks>
        public ScheduleInfoCollection List(PagingFilter filter = null)
        {
            return RestClient.Get<ScheduleInfoCollection>(_client.Config, EndPoint, filter);
        }

        public ScheduleIdCollection Create(string codeName, string payload, ScheduleOptions options)
        {
            return Create(new SchedulePayloadCollection(codeName, payload, options));
        }

        public ScheduleIdCollection Create(SchedulePayloadCollection collection)
        {
            return RestClient.Post<ScheduleIdCollection>(_client.Config, EndPoint, collection);
        }

        public ScheduleInfo Get(string scheduleId)
        {
            return RestClient.Get<ScheduleInfo>(_client.Config, ScheduleEndPoint(scheduleId));
        }

        public bool Cancel(string scheduleId)
        {
            return RestClient.Post<ResponseMsg>(_client.Config, ScheduleEndPoint(scheduleId) + "/cancel").HasExpectedMessage("Cancelled");
        }
    }
}