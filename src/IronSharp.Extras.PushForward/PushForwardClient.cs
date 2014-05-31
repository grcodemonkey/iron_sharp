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

        public async Task<PushForwardQueueClient> Queue<T>(PushForwardConfig config = null)
        {
            return await Queue(QueueNameAttribute.GetName<T>(), config);
        }

        public async Task<PushForwardQueueClient> Queue(string name, PushForwardConfig config = null)
        {
            LazyInitializer.EnsureInitialized(ref config, ()=> new PushForwardConfig
            {
                PushType = PushStyle.Multicast,
                Retries = 3,
                RetryDelay = TimeSpan.FromSeconds(60),
                ErrorQueueName = string.Format("{0}_Errors", name)
            });

            QueueClient queueClient = IronMqClient.Queue(name);

            QueueInfo queueInfo = await queueClient.Info();

            bool requiresPushTypeUpdate = RequiresPushTypeUpdate(queueInfo, config.PushType);

            bool requiresErrorQueueUpdate = RequiresErrorQueueUpdate(queueInfo, config);

            bool requiresRetryUpdate = RequiresRetryUpdate(queueInfo, config);

            bool requiresRetryDelayUpdate = RequiresRetryDelayUpdate(queueInfo, config);

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

        private static bool RequiresRetryDelayUpdate(QueueInfo queueInfo, PushForwardConfig config)
        {
            if (config.Retries == null)
            {
                return false;
            }

            return queueInfo.Retries != config.Retries.Value;
        }

        private static bool RequiresRetryUpdate(QueueInfo queueInfo, PushForwardConfig config)
        {
            if (config.RetryDelay == null)
            {
                return false;
            }

            return queueInfo.RetriesDelay != config.RetryDelay.Value.Seconds;
        }

        private static bool RequiresErrorQueueUpdate(QueueInfo queueInfo, PushForwardConfig config)
        {
            if (string.IsNullOrEmpty(config.ErrorQueueName))
            {
                return false;
            }
            return !string.Equals(queueInfo.ErrorQueue, config.ErrorQueueName);
        }

        private static bool RequiresPushTypeUpdate(QueueInfo queueInfo, PushStyle pushStyle)
        {
            switch (queueInfo.PushType)
            {
                case PushType.Pull:
                    return true;
                case PushType.Multicast:
                    return pushStyle == PushStyle.Unicast;
                case PushType.Unicast:
                    return pushStyle == PushStyle.Multicast;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}