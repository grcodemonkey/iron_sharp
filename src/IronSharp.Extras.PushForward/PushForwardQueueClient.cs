using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using IronSharp.Core;
using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{
    public class PushForwardQueueClient
    {
        private readonly QueueClient _queueClient;
        private readonly QueueInfo _queueInfo;

        public PushForwardQueueClient(QueueClient queueClient, QueueInfo queueInfo)
        {
            _queueClient = queueClient;
            _queueInfo = queueInfo;
        }

        public async Task<QueueInfo> SetErrorQueue(string errorQueueName)
        {
            return await _queueClient.Update(new QueueInfo
              {
                  ErrorQueue = errorQueueName
              });
        }

        public async Task<QueueInfo> AddSubscriber(EndPointConfig endPoint, string path, NameValueCollection query)
        {
            Uri endPointUri = SubscriberItemBuilder.BuildUri(endPoint, path, query);
            return await AddSubscriber(SubscriberItemBuilder.GetSubscriberInfo(endPointUri, endPoint.Headers));
        }

        public async Task<QueueInfo> AddSubscriber(SubscriberItem subscriber)
        {

            return await _queueClient.AddSubscribers(new SubscriberRequestCollection
              {
                  Subscribers = new List<SubscriberItem> { subscriber }
              });
        }

        public async Task<QueuedMessageResult> QueuePushMessage(object payload, MessageOptions messageOptions = null)
        {
            string messageId = await _queueClient.Post(payload, messageOptions);

            return new QueuedMessageResult
            {
                MessageId = messageId,
                Success = !string.IsNullOrEmpty(messageId)
            };
        }
    }
}