using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IronSharp.Core;
using IronSharp.Extras.PushForward;
using IronSharp.IronMQ;

namespace Demo.IronSharpConsole
{
    internal static class IronMqExample
    {
        public static async Task Run()
        {
            // =========================================================
            // Iron.io MQ
            // =========================================================

            IronMqRestClient ironMq = IronSharp.IronMQ.Client.New();

            // Get a Queue object
            QueueClient queue = ironMq.Queue("my_queue");

            QueueInfo info = await queue.Info();

            Console.WriteLine(info.Inspect());

            // Put a message on the queue
            string messageId = await queue.Post("hello world!");

            Console.WriteLine(messageId);

            // Use a webhook to post message from a third party
            Uri webhookUri = queue.WebhookUri();

            Console.WriteLine(webhookUri);

            // Get a message
            QueueMessage msg = await queue.Next();

            Console.WriteLine(msg.Inspect());

            //# Delete the message
            bool deleted = await msg.Delete();

            Console.WriteLine("Deleted = {0}", deleted);

            var payload1 = new
            {
                message = "hello, my name is Iron.io 1"
            };

            var payload2 = new
            {
                message = "hello, my name is Iron.io 2"
            };

            var payload3 = new
            {
                message = "hello, my name is Iron.io 3"
            };

            MessageIdCollection queuedUp = await queue.Post(new[] {payload1, payload2, payload3});

            Console.WriteLine(queuedUp.Inspect());

            QueueMessage next;

            while (queue.Read(out next))
            {
                Console.WriteLine(next.Inspect());
                Console.WriteLine("Deleted = {0}", await next.Delete());
            }
        }
    }
}