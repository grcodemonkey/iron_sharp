using System;
using IronSharp.Core;
using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{
    public class PushForwardConfig : IInspectable
    {
        public string ErrorQueueName { get; set; }

        /// <summary>
        /// Default is Multicast
        /// </summary>
        public PushStyle PushType { get; set; }

        /// <summary>
        /// Maximum is 100 - Default is 3
        /// </summary>
        public int? Retries { get; set; }

        /// <summary>
        /// Maximum is 24 hrs - default is 60 seconds
        /// </summary>
        public TimeSpan? RetryDelay { get; set; }

        public PushType GetPushType()
        {
            return PushType == PushStyle.Multicast ? IronMQ.PushType.Multicast : IronMQ.PushType.Unicast;
        }

        public void SetPushType(PushType type)
        {
            if (type == IronMQ.PushType.Unicast)
            {
                PushType = PushStyle.Unicast;
            }
            else
            {
                PushType = PushStyle.Multicast;
            }
        }
    }
}