using System.Net.Http;
using System.Threading.Tasks;

namespace IronSharp.Core
{
    public class RestResponse<T>
    {
        public RestResponse(HttpResponseMessage responseMessage)
        {
            ResponseMessage = responseMessage;
        }

        public T Result
        {
            get { return ReadResultAsync().Result; }
        }

        public Task<T> ReadResultAsync()
        {
            return ResponseMessage.Content.ReadAsAsync<T>();
        }

        public HttpResponseMessage ResponseMessage { get; set; }

        public ResponseMsg Msg()
        {
            return ResponseMessage.Content.ReadAsAsync<ResponseMsg>().Result;
        }

        public bool HasExpectedMessage(string message)
        {
            var msg = Msg();
            return msg != null && msg.Message == message;
        }

        public static implicit operator bool(RestResponse<T> value)
        {
            return value.ResponseMessage != null && value.ResponseMessage.IsSuccessStatusCode;
        }

        public static implicit operator T(RestResponse<T> value)
        {
            return value.Result;
        }
    }
}