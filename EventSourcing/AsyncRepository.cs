using System.Threading.Tasks;

namespace EventSourcing
{
    public interface IAsyncRepository<TAggregateRoot>
    {
        Task<TAggregateRoot> GetAggregate(string aggregateId);
        Task SaveAggregate(TAggregateRoot aggregate);
    }
    
    public class AsyncRepository<TAggregateRoot> : IAsyncRepository<TAggregateRoot>
        where TAggregateRoot : Aggregate, new()
    {
        private readonly IEventStore _eventStore;

        public AsyncRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        
        public async Task<TAggregateRoot> GetAggregate(string aggregateId)
        {
            var streamId = GetStreamId(aggregateId);
            
            var root = new TAggregateRoot();
            
            await root.Initialize(_eventStore.GetEventsFromStream(streamId), _eventStore.GetLastEventNumber(streamId));

            return root;
        }

        public async Task SaveAggregate(TAggregateRoot aggregate)
        {
            if (!aggregate.HasChanges())
                return;

            var streamId = GetStreamId(aggregate.AggregateId);
            var changes = aggregate.GetChanges();
            if (aggregate.AggregateIsNotPersisted())
                await _eventStore.CreateStream(streamId, changes);
            else
                await _eventStore.AppendStream(streamId, changes, aggregate.AggregateVersion);
        }

        private string GetStreamId(string aggregateId) => $"{typeof(TAggregateRoot).FullName}-{aggregateId}";
    }
}