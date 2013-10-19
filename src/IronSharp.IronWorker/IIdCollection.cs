using System.Collections.Generic;

namespace IronSharp.IronWorker
{
    public interface IIdCollection
    {
        IEnumerable<string> GetIds();
    }
}