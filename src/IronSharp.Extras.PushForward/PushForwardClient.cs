using System;
using System.Threading.Tasks;
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

        public async Task<PushForwardQueueClient> Queue<T>(bool multicast = true)
        {
            QueueClient<T> queueClient = _ironMq.Queue<T>();

            return new PushForwardQueueClient(queueClient, multicast);
        }

        public async Task<PushForwardQueueClient> Queue(string name, bool multicast = true)
        {
            QueueClient queueClient = _ironMq.Queue(name);

            QueueInfo queueInfo = await queueClient.Info();

            switch (queueInfo.PushType)
            {
                case PushType.Pull:
                    queueInfo = await queueClient.Update(new QueueInfo
                    {
                        PushType = multicast ? PushType.Multicast : PushType.Unicast
                    });

                    break;
                case PushType.Multicast:
                    if (!multicast)
                    {
                        queueInfo = await queueClient.Update(new QueueInfo
                        {
                            PushType = PushType.Unicast
                        });
                    }
                    break;
                case PushType.Unicast:
                    if (multicast)
                    {
                        queueInfo = await queueClient.Update(new QueueInfo
                        {
                            PushType = PushType.Multicast
                        });
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new PushForwardQueueClient(queueClient, queueInfo);
        }
    }
}