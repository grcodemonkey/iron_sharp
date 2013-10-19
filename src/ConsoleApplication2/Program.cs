using System;
using IronSharp.Core;
using IronSharp.IronCache;
using IronSharp.IronMQ;
using IronSharp.IronWorker;

namespace ConsoleApplication2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string projectId = "INSERT_PROJECT_ID";
            string token = "TOKEN_GOES_HERE";

            // =========================================================
            // Iron.io Cache
            // =========================================================

            IronCacheRestClient ironCacheClient = IronSharp.IronCache.Client.New(projectId, token);

            // Get a Cache object
            CacheClient cache = ironCacheClient.Cache("my_cache");

            // Put value to cache by key
            cache.Put("number_item", 42);

            // Get value from cache by key
            Console.WriteLine(cache.Get("number_item").Value);

            // Get value from cache by key
            Console.WriteLine(cache.Get<int>("number_item"));

            // Numbers can be incremented
            cache.Increment("number_item", 10);

            // Immediately delete an item
            cache.Delete("number_item");

            // =========================================================
            // Iron.io MQ
            // =========================================================

            IronMqRestClient ironMq = IronSharp.IronMQ.Client.New(projectId, token);

            // Get a Queue object
            QueueClient queue = ironMq.Queue("my_queue");

            // Put a message on the queue
            MessageIdCollection result = @queue.Post("hello world!");

            Console.WriteLine(result.Inspect());

            // Get a message
            QueueMessage msg = queue.Next();

            Console.WriteLine(msg.Inspect());
            
            //# Delete the message
            bool deleted = msg.Delete();

            Console.WriteLine("Deleted = {0}", deleted);

            QueueMessage next;

            while (queue.Read(out next))
            {
                Console.WriteLine(next.Inspect());
                Console.WriteLine(next.Delete());
            }
            
            // =========================================================
            // Iron.io Worker
            // =========================================================

            IronWorkerRestClient workerClient = IronSharp.IronWorker.Client.New(projectId, token);

            //string taskId = workerClient.Tasks.Create("Test", new { Key = "Value" });

            //Console.WriteLine("TaskID: {0}", taskId);

            //TaskInfoCollection taskInfoCollection = workerClient.Tasks.List("Test");

            //foreach (var task in taskInfoCollection.Tasks)
            //{
            //    Console.WriteLine(task.Inspect());
            //}

            ScheduleOptions options = ScheduleBuilder.Build().
                                            Delay(TimeSpan.FromMinutes(1)).
                                            WithPriority(TaskPriority.High);

            ScheduleIdCollection schedule= workerClient.Schedules.Create("Test", new[] {1, 2, 3, 4, 5}, options);

            Console.WriteLine(schedule.Inspect());

            Console.WriteLine("============= Done ==============");
            Console.Read();
        }
    }
}