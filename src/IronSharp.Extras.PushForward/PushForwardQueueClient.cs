using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IronSharp.Core;
using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{
    public class PushForwardQueueClient
    {
        private readonly PushForwardClient _client;
        private readonly QueueClient _queueClient;
        private IFailedMessageRetrySender _failedMessageRetrySender;

        public PushForwardQueueClient(PushForwardClient client, QueueClient queueClient, QueueInfo queueInfo)
        {
            QueueInfo = queueInfo;
            _client = client;
            _queueClient = queueClient;
        }

        public IFailedMessageRetrySender FailedMessageRetrySender
        {
            get { return LazyInitializer.EnsureInitialized(ref _failedMessageRetrySender, () => new FailedMessageRetrySender(_client, _queueClient)); }
            set { _failedMessageRetrySender = value; }
        }

        public QueueInfo QueueInfo { get; set; }

        public async Task AddSubscriber(EndPointConfig endPoint, string path, NameValueCollection query = null)
        {
            await AddSubscriber(SubscriberItemBuilder.GetSubscriberItem(endPoint, path, query));
        }

        public async Task AddSubscriber(string subscriberUrl)
        {
            await AddSubscriber(new SubscriberItem(subscriberUrl));
        }

        public async Task AddSubscriber(SubscriberItem subscriber)
        {
            await _queueClient.AddSubscribers(new SubscriberRequestCollection
            {
                Subscribers = new List<SubscriberItem> { subscriber }
            });

            QueueInfo = await _queueClient.Info();
        }

        public Uri GetWebhookUri(string token = null)
        {
            return _queueClient.WebhookUri(token);
        }

        public bool HasSubscriber(EndPointConfig endPoint, string path, NameValueCollection query = null)
        {
            return HasSubscriber(SubscriberItemBuilder.GetSubscriberItem(endPoint, path, query));
        }

        public bool HasSubscriber(string subscriberUrl)
        {
            return HasSubscriber(new SubscriberItem(subscriberUrl));
        }

        public bool HasSubscriber(SubscriberItem subscriber)
        {
            return QueueInfo.Subscribers.Any(x => SubscriberItem.SubscriberItemComparer.Equals(x, subscriber));
        }

        public async Task<MessageIdCollection> QueuePushMessage(IEnumerable<object> payloads, MessageOptions messageOptions = null)
        {
            return await _queueClient.Post(payloads, messageOptions);
        }

        public async Task<MessageIdCollection> QueuePushMessage(object payload, MessageOptions messageOptions = null)
        {
            return await QueuePushMessage(new[] {payload}, messageOptions);
        }

        public async Task RemoveSubscriber(EndPointConfig endPoint, string path, NameValueCollection query = null)
        {
            await RemoveSubscriber(SubscriberItemBuilder.GetSubscriberItem(endPoint, path, query));
        }

        public async Task RemoveSubscriber(string subscriberUrl)
        {
            await RemoveSubscriber(new SubscriberItem(subscriberUrl));
        }

        public async Task RemoveSubscriber(SubscriberItem subscriber)
        {
            await _queueClient.RemoveSubscribers(new SubscriberRequestCollection
            {
                Subscribers = new List<SubscriberItem> { subscriber }
            });

            QueueInfo = await _queueClient.Info();
        }

        public async Task ReplaceSubscribers(EndPointConfig endPoint, string path, NameValueCollection query = null)
        {
            await ReplaceSubscribers(SubscriberItemBuilder.GetSubscriberItem(endPoint, path, query));
        }

        public async Task ReplaceSubscribers(string subscriberUrl)
        {
            await ReplaceSubscribers(new SubscriberItem(subscriberUrl));
        }

        public async Task ReplaceSubscribers(SubscriberItem subscriberItem)
        {
            await ReplaceSubscribers(new List<SubscriberItem> { subscriberItem });
        }

        public async Task ReplaceSubscribers(List<SubscriberItem> subscribers)
        {
            QueueInfo = await _queueClient.Update(new QueueInfo
            {
                PushType = QueueInfo.PushType,
                Subscribers = subscribers
            });

            QueueInfo = await _queueClient.Info();
        }

        public async Task<MessageIdCollection> ResendFailedMessages(int? limit = null)
        {
            return await ResendFailedMessages(CancellationToken.None, limit);
        }

        public async Task<MessageIdCollection> ResendFailedMessages(CancellationToken cancellationToken, int? limit = null)
        {
            return await FailedMessageRetrySender.ResendFailedMessages(cancellationToken, limit);
        }

        public async Task SetErrorQueue(string errorQueueName, Alert alert = null)
        {
            QueueInfo = await _queueClient.Info();

            QueueInfo = await _queueClient.Update(new QueueInfo
            {
                ErrorQueue = errorQueueName
            });

            if (alert != null)
            {
                await _client.AddAlertToErrorQueue(errorQueueName, alert);
            }
        }


    }
}