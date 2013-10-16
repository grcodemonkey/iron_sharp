using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using IronSharp.Core;

namespace IronSharp.IronMQ
{
    public class QueueClient
    {
        private readonly Client _client;
        private readonly string _name;

        public QueueClient(Client client, string name)
        {
            _client = client;
            _name = name;
        }

        public string EndPoint
        {
            get { return string.Format("{0}/{1}", _client.EndPoint, _name); }
        }

        public bool Clear()
        {
            return RestClient.Post<ResponseMsg>(_client.Config, string.Format("{0}/clear", EndPoint)).HasExpectedMessage("Cleared.");
        }

        public bool Delete()
        {
            return RestClient.Delete<ResponseMsg>(_client.Config, EndPoint).HasExpectedMessage("Deleted.");
        }

        public bool Delete(string messageId)
        {
            return RestClient.Delete<ResponseMsg>(_client.Config, string.Format("{0}/messages/{1}", EndPoint, messageId)).HasExpectedMessage("Deleted");
        }

        public bool Delete(IEnumerable<string> messageIds)
        {
            return RestClient.Delete<ResponseMsg>(_client.Config, string.Format("{0}/messages", EndPoint), payload: new MessageIdCollection(messageIds)).HasExpectedMessage("Deleted");
        }

        public QueueMessage Get(string messageId)
        {
            return RestClient.Get<QueueMessage>(_client.Config, string.Format("{0}/messages/{1}", EndPoint, messageId));
        }

        public MessageCollection Get(int? n = null, int? timeout = null)
        {
            var query = new NameValueCollection();

            if (n.HasValue)
            {
                query.Add("n", Convert.ToString(n));
            }

            if (timeout.HasValue)
            {
                query.Add("timeout", Convert.ToString(timeout));
            }

            return RestClient.Get<MessageCollection>(_client.Config, string.Format("{0}/messages", EndPoint), query);
        }

        public QueueInfo Info()
        {
            return RestClient.Get<QueueInfo>(_client.Config, EndPoint);
        }

        public QueueMessage Next(int? timeout = null)
        {
            return Get(1, timeout).Messages.FirstOrDefault();
        }

        public MessageCollection Peek(int? n = null)
        {
            var query = new NameValueCollection();

            if (n.HasValue)
            {
                query.Add("n", Convert.ToString(n));
            }

            return RestClient.Get<MessageCollection>(_client.Config, string.Format("{0}/messages/peek", EndPoint), query);
        }

        public QueueMessage PeekNext()
        {
            return Peek(1).Messages.FirstOrDefault();
        }

        public dynamic Post(QueueMessage message)
        {
            return Post(new MessageCollection(message));
        }

        public dynamic Post(IEnumerable<QueueMessage> messages)
        {
            return Post(new MessageCollection(messages));
        }

        public dynamic Post(object message)
        {
            return Post(new MessageCollection(message));
        }

        public dynamic Post(IEnumerable<object> messages)
        {
            return Post(new MessageCollection(messages));
        }

        public dynamic Post(string message)
        {
            return Post(new MessageCollection(message));
        }

        public dynamic Post(IEnumerable<string> messages)
        {
            return Post(new MessageCollection(messages));
        }

        public dynamic Post(MessageCollection messageCollection)
        {
            return RestClient.Post<dynamic>(_client.Config, string.Format("{0}/messages", EndPoint), messageCollection);
        }

        public bool Release(string messageId, int? delay = null)
        {
            var query = new NameValueCollection();

            if (delay.HasValue)
            {
                query.Add("delay", Convert.ToString(delay));
            }

            return RestClient.Post<ResponseMsg>(_client.Config, string.Format("{0}/messages/{1}/release", EndPoint, messageId), query).HasExpectedMessage("Released");
        }

        public bool Touch(string messageId)
        {
            return RestClient.Post<ResponseMsg>(_client.Config, string.Format("{0}/messages/{1}/touch", EndPoint, messageId)).HasExpectedMessage("Touched");
        }
        public QueueInfo Update(QueueInfo updates)
        {
            return RestClient.Post<QueueInfo>(_client.Config, EndPoint, updates);
        }
    }
}