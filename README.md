![Fe#](http://c577730.r99.cf2.rackcdn.com/images/FeSharp.png)
==========

# IronSharp is a .NET client for [Iron.io](http://www.iron.io/)


## Getting Started

1. Sign up at <https://hud.iron.io/users/new>
2. Get your credentials from [https://hud.iron.io/dashboard](https://hud.iron.io/dashboard "Heads up")
3. Check out the [wiki](https://github.com/grcodemonkey/iron_sharp/wiki "For those that like to read directions")

## IronCache
<http://dev.iron.io/cache/>

```PM> Install-Package IronSharp.IronCache```

```C#
// =========================================================
// Iron.io Cache
// =========================================================

IronCacheRestClient ironCacheClient = IronSharp.IronCache.Client.New();

// Get a Cache object
CacheClient cache = ironCacheClient.Cache("my_cache");

// Put value to cache by key
await cache.Put("number_item", 42);

CacheItem item =  await cache.Get("number_item");

// Get value from cache by key
Console.WriteLine(item.Value);

// Get value from cache by key
Console.WriteLine(await cache.Get<int>("number_item"));

// Numbers can be incremented
await cache.Increment("number_item", 10);

// Immediately delete an item
await cache.Delete("number_item");

await cache.Put("complex_item", new { greeting = "Hello", target = "world" });

CacheItem complexItem = await cache.Get("complex_item");

// Get value from cache by key
Console.WriteLine(complexItem.Value);

await cache.Delete("complex_item");
```

## IronMQ
<http://dev.iron.io/mq/>

```PM> Install-Package IronSharp.IronMQ```

```C#
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

MessageIdCollection queuedUp = await queue.Post(new[] { payload1, payload2, payload3 });

Console.WriteLine(queuedUp.Inspect());

QueueMessage next;

while (queue.Read(out next))
{
    Console.WriteLine(next.Inspect());
    Console.WriteLine("Deleted = {0}", await next.Delete());
}
```

## IronWorker
<http://dev.iron.io/worker/>

```PM> Install-Package IronSharp.IronWorker```

```C#
// =========================================================
// Iron.io Worker
// =========================================================

IronWorkerRestClient workerClient = IronSharp.IronWorker.Client.New();

string taskId = await workerClient.Tasks.Create("Test", new { Key = "Value" });

Console.WriteLine("TaskID: {0}", taskId);

TaskInfoCollection taskInfoCollection = await workerClient.Tasks.List("Test");

foreach (TaskInfo task in taskInfoCollection.Tasks)
{
    Console.WriteLine(task.Inspect());
}

ScheduleOptions options = ScheduleBuilder.Build().
    Delay(TimeSpan.FromMinutes(1)).
    WithFrequency(TimeSpan.FromHours(1)).
    RunFor(TimeSpan.FromHours(3)).
    WithPriority(TaskPriority.Default);

var payload = new
{
    a = "b",
    c = new[] { 1, 2, 3 }
};

ScheduleIdCollection schedule = await workerClient.Schedules.Create("Test", payload, options);

Console.WriteLine(schedule.Inspect());

await workerClient.Schedules.Cancel(schedule.Schedules.First().Id);
```
