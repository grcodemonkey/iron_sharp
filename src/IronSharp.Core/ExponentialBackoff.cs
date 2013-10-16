using System;
using System.Threading;

namespace IronSharp.Core
{
    internal static class ExponentialBackoff
    {
        public static void Sleep(int attempt)
        {
            Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(3.0, attempt)));
        }
    }
}