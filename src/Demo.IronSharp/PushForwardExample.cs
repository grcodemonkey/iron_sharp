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

            string wrongUrl = endPointUrl + "notexists.wrong";

            await pushQueue.SetErrorQueue("ErrorQ", new Alert
            {
                Direction = AlertDirection.Asc,
                Queue = "Send Admin Alert",
                Trigger = 1
            });

            if (!pushQueue.HasSubscriber(wrongUrl))
            {
                await pushQueue.AddSubscriber(new SubscriberItem
                {
                    Url = wrongUrl,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json"}
                    }
                });

                Console.WriteLine("Subscriber added");
                Console.WriteLine(pushQueue.QueueInfo.Inspect());
                Console.ReadLine();
            }

            MessageIdCollection queuedUp = await pushQueue.QueuePushMessage(new
            {
                message = "hello, my name is Push Forward",
                endPointUrl,
                guid = Guid.NewGuid()
            });

            Console.WriteLine(queuedUp.Inspect());

            Console.WriteLine("Message pushed to bad end point");
            Console.ReadLine();

            await pushQueue.ReplaceSubscribers(endPointUrl);

            Console.WriteLine("End point fixed");
            Console.ReadLine();

            MessageIdCollection resentMessages = await pushQueue.ResendFailedMessages();

            Console.WriteLine(resentMessages.Inspect());
        }
    }
}