using System.Collections.Generic;
using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{

    public class PushForwardQueueInfo
    {

        public string QueueName { get; set; }

        public List<SubscriberItem> Subscribers { get; set; }

        public PushType PushType { get; set; }

        public string ErrorQueue { get; set; }
    }
}