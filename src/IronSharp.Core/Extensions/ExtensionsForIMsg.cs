using System.Threading.Tasks;

namespace IronSharp.Core
{
    public static class ExtensionsForIMsg
    {
        public static bool HasExpectedMessage(this IMsg msg, string message)
        {
            return msg != null && msg.Message == message;
        }

        public async static Task<bool> HasExpectedMessage(this Task<RestResponse<ResponseMsg>> msg, string message)
        {
            return await msg != null && msg.Result.HasExpectedMessage(message);
        }
    }
}