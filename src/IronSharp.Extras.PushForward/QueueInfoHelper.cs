using System;
using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{
    internal static class QueueInfoHelper
    {
        public static bool RequiresRetryDelayUpdate(QueueInfo queueInfo, PushForwardConfig config)
        {
            if (config.Retries == null)
            {
                return false;
            }

            return queueInfo.Retries != config.Retries.Value;
        }

        public static bool RequiresRetryUpdate(QueueInfo queueInfo, PushForwardConfig config)
        {
            if (config.RetryDelay == null)
            {
                return false;
            }

            return queueInfo.RetriesDelay != config.RetryDelay.Value.Seconds;
        }

        public static bool RequiresErrorQueueUpdate(QueueInfo queueInfo, PushForwardConfig config)
        {
            if (String.IsNullOrEmpty(config.ErrorQueueName))
            {
                return false;
            }
            return !String.Equals(queueInfo.ErrorQueue, config.ErrorQueueName);
        }

        public static bool RequiresPushTypeUpdate(QueueInfo queueInfo, PushStyle pushStyle)
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