using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using IronSharp.Core;
using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{
    public class PushForwardQueueClient
    {
        private readonly PushForwardClient _client;
        private readonly QueueClient _queueClient;
        
        public PushForwardQueueClient(PushForwardClient client, QueueClient queueClient, QueueInfo queueInfo)
        {
            QueueInfo = queueInfo;
            _client = client;
            _queueClient = queueClient;
        }

        public QueueInfo QueueInfo { get; set; }

        public async Task SetErrorQueue(string errorQueueName)
        {
            if (string.Equals(QueueInfo.ErrorQueue, errorQueueName))
            {
                return;
            }

            QueueInfo = await _queueClient.Update(new QueueInfo
            {
                ErrorQueue = errorQueueName
            });
        }

        public Uri GetWebhookUri(string token = null)
        {
            return _queueClient.WebhookUri(token);
        }

        public async Task AddSubscriber(EndPointConfig endPoint, string path, NameValueCollection query)
        {
            await AddSubscriber(SubscriberItemBuilder.GetSubscriberItem(endPoint, path, query));
        }

        public async Task AddSubscriber(SubscriberItem subscriber)
        {
            QueueInfo = await _queueClient.Info();

            if (QueueInfo.Subscribers.Any(x => SubscriberItem.SubscriberItemComparer.Equals(x, subscriber)))
            {
                return;
            }

            QueueInfo = await _queueClient.AddSubscribers(new SubscriberRequestCollection
            {
                Subscribers = new List<SubscriberItem> {subscriber}
            });
        }

        public async Task RemoveSubscriber(EndPointConfig endPoint, string path, NameValueCollection query)
        {
            await RemoveSubscriber(SubscriberItemBuilder.GetSubscriberItem(endPoint, path, query));
        }

        public async Task RemoveSubscriber(SubscriberItem subscriber)
        {
            QueueInfo = await _queueClient.Info();

            if (QueueInfo.Subscribers.Any(x => SubscriberItem.SubscriberItemComparer.Equals(x, subscriber)))
            {
                QueueInfo = await _queueClient.RemoveSubscribers(new SubscriberRequestCollection
                {
                    Subscribers = new List<SubscriberItem> { subscriber }
                });
            }           
        }

        public async Task ResendFailedMessages()
        {
            string errorQueueName = QueueInfo.ErrorQueue;

            if (string.IsNullOrEmpty(errorQueueName))
            {
                return;
            }

            QueueClient errorQ = _client.IronMqClient.Queue(errorQueueName);

            QueueMessage next;

            while (errorQ.Read(out next))
            {
                await _queueClient.Post(next);
                await  next.Delete();
            }
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