using System.Threading;
using System.Threading.Tasks;
using IronSharp.IronMQ;

namespace IronSharp.Extras.PushForward
{
    public interface IFailedMessageRetrySender
    {
        Task<MessageIdCollection> ResendFailedMessages(CancellationToken cancellationToken, int? limit = null);
    }
}