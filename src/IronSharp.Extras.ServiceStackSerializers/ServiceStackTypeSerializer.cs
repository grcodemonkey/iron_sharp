using IronSharp.Core;
using ServiceStack.Text;

namespace IronSharp.Extras.ServiceStackSerializers
{
    public class ServiceStackTypeSerializer : IValueSerializer
    {
        public string Generate(object value)
        {
            return TypeSerializer.SerializeToString(value);
        }

        public T Parse<T>(string value)
        {
            return TypeSerializer.DeserializeFromString<T>(value);
        }
    }
}