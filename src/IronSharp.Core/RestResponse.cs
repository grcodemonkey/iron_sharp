using System.Net.Http;
using System.Threading.Tasks;

namespace IronSharp.Core
{
    public class RestResponse<T> : IMsg
    {
        public RestResponse(HttpResponseMessage responseMessage)
        {
            ResponseMessage = responseMessage;
        }

        string IMsg.Message
        {
            get
            {
                var msg = Msg();
                return msg == null ? null : msg.Message;
            }
        }

        public HttpResponseMessage ResponseMessage { get; set; }

        public T Result
        {
            get { return ReadResultAsync().Result; }
        }

        public static implicit operator bool(RestResponse<T> value)
        {
            return value.ResponseMessage != null && value.ResponseMessage.IsSuccessStatusCode;
        }

        public static implicit operator T(RestResponse<T> value)
        {
            return value.Result;
        }

        public ResponseMsg Msg()
        {
            return ResponseMessage.Content.ReadAsAsync<ResponseMsg>().Result;
        }

        public Task<T> ReadResultAsync()
        {
            return ResponseMessage.Content.ReadAsAsync<T>();
        }
    }
}