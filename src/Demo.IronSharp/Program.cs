using System;
using Common.Logging;
using Common.Logging.Simple;

namespace Demo.IronSharpConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();

            IronMqExample.Run().Wait();

            PushForwardExample.Run().Wait();

            IronCachExample.Run().Wait();

            IronWorkerExample.Run().Wait();

            Console.WriteLine("============= Done ==============");
            Console.Read();
        }
    }
}