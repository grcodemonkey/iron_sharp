using System.Threading;

namespace IronSharp.Core
{
    public class IronSharpConfig: IInspectable
    {
        private IValueSerializer _valueSerializer;

        public double BackoffFactor { get; set; }

        public IValueSerializer ValueSerializer
        {
            get { return LazyInitializer.EnsureInitialized(ref _valueSerializer, ()=> new DefaultValueSerializer()); }
            set { _valueSerializer = value; }
        }
    }
}