using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IronSharp.Core;
using IronSharp.Extras.PushForward;
using IronSharp.IronMQ;

namespace Demo.IronSharpConsole
{
    internal static class PushForwardExample
    {
        public static async Task Run()
        {
            Console.WriteLine("Go to http://requestb.in/ and click on \"Create a RequestBin\" to get a test URL");
            Console.WriteLine("Enter testing URL:");
            string endPointUrl = Console.ReadLine();

            IronMqRestClient ironMq = IronSharp.IronMQ.Client.New();

            PushForwardClient pushForwardClient = IronSharp.Extras.PushForward.Client.New(ironMq);

            PushForwardQueueClient pushQueue = await pushForwardClient.PushQueue("PushForward");

            if (!pushQueue.HasSubscriber(endPointUrl))
            {
                await pushQueue.AddSubscriber(new SubscriberItem
                {
                    Url = endPointUrl,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json"}
                    }
                });
            }

            MessageIdCollection queuedUp = await pushQueue.QueuePushMessage(new
            {
                message = "hello, my name is Push Forward",
                endPointUrl
            });

            Console.WriteLine(queuedUp.Inspect());
        }
    }
}