using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing
{
    public interface IEventStore
    {
        Task CreateStream(string streamId, List<object> events);
        Task AppendStream(string streamId, List<object> events, int expectedVersion);
        IAsyncEnumerable<object> GetEventsFromStream(string streamId);
        Task<int> GetLastEventNumber(string streamId);
    }
}