using System;
using System.Threading;
using System.Threading.Tasks;
using IronSharp.Core.Attributes;
using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{
    public class PushForwardClient
    {
        private readonly IronMqRestClient _ironMq;

        public PushForwardClient(IronMqRestClient ironMq)
        {
            _ironMq = ironMq;
        }

        public IronMqRestClient IronMqClient
        {
            get { return _ironMq; }
        }

        public async Task<PushForwardQueueClient> PushQueue<T>(PushForwardConfig config = null)
        {
            return await PushQueue(QueueNameAttribute.GetName<T>(), config);
        }

        public async Task<PushForwardQueueClient> PushQueue(string name, PushForwardConfig config = null)
        {
            LazyInitializer.EnsureInitialized(ref config, () => new PushForwardConfig
            {
                PushType = PushStyle.Multicast,
                Retries = 3,
                RetryDelay = TimeSpan.FromSeconds(60),
                ErrorQueueName = string.Format("{0}_Errors", name)
            });

            QueueClient queueClient = IronMqClient.Queue(name);

            QueueInfo queueInfo = await queueClient.Info();

            bool requiresPushTypeUpdate = QueueInfoHelper.RequiresPushTypeUpdate(queueInfo, config.PushType);

            bool requiresErrorQueueUpdate = QueueInfoHelper.RequiresErrorQueueUpdate(queueInfo, config);

            bool requiresRetryUpdate = QueueInfoHelper.RequiresRetryUpdate(queueInfo, config);

            bool requiresRetryDelayUpdate = QueueInfoHelper.RequiresRetryDelayUpdate(queueInfo, config);

            var update = new QueueInfo();

            bool shouldUpdate = false;

            if (requiresPushTypeUpdate)
            {
                shouldUpdate = true;
                update.PushType = config.PushType == PushStyle.Multicast ? PushType.Multicast : PushType.Unicast;
            }

            if (requiresErrorQueueUpdate)
            {
                shouldUpdate = true;
                update.ErrorQueue = config.ErrorQueueName;
            }

            if (requiresRetryUpdate)
            {
                shouldUpdate = true;
                update.Retries = config.Retries;
            }

            if (requiresRetryDelayUpdate)
            {
                shouldUpdate = true;
                update.RetriesDelay = config.RetryDelay.GetValueOrDefault().Seconds;
            }

            if (shouldUpdate)
            {
                queueInfo = await queueClient.Update(update);
            }

            return new PushForwardQueueClient(this, queueClient, queueInfo);
        }

        public async Task AddAlertToErrorQueue(string errorQueueName, Alert alert)
        {
            QueueClient errorQ = _ironMq.Queue(errorQueueName);

            await errorQ.AddAlert(alert);
        }
    }
}