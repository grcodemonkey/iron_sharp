using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{
    public static class Client
    {
        public static PushForwardClient @New(IronMqRestClient ironMq)
        {
            return new PushForwardClient(ironMq);
        }
    }
}