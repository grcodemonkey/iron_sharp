using System;
using System.Linq;
using System.Threading.Tasks;
using IronSharp.Core;
using IronSharp.IronWorker;

namespace Demo.IronSharpConsole
{
    static internal class IronWorkerExample
    {
        public async static Task Run()
        {
            // =========================================================
            // Iron.io Worker
            // =========================================================

            Console.WriteLine("Be sure to create a 'Test' worker before running this sample");
            Console.WriteLine("Press ANY key to continue");
            Console.Read();

            IronWorkerRestClient workerClient = Client.New();

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
        }
    }
}