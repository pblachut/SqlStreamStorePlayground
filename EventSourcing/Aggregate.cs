using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing
{
    public abstract class Aggregate
    {
        private const int NotPersistedAggregateVersion = -1;
        
        private readonly List<object> _recordedEvents;
        private readonly Dictionary<Type, Action<object>> _eventHandlers;
        
        public int AggregateVersion { get; private set; }
        public string AggregateId { get; protected set; }

        protected Aggregate()
        {
            AggregateVersion = NotPersistedAggregateVersion;
            _eventHandlers = new Dictionary<Type, Action<object>>();
            _recordedEvents = new List<object>();
        }

        public bool AggregateIsNotPersisted() => AggregateVersion == NotPersistedAggregateVersion;

        protected void On<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) 
                throw new ArgumentNullException(nameof(handler));

            _eventHandlers.Add(typeof (TEvent), @event => handler((TEvent) @event));
        }

        public async Task Initialize(IAsyncEnumerable<object> events, Task<int> aggregateVersion)
        {
            if (HasChanges())
                throw new InvalidOperationException("Initialize cannot be called on an instance with changes.");
            
            await foreach (var @event in events)
            {
                Play(@event);
            }
            
            AggregateVersion = await aggregateVersion;
        }
        
//        public void Initialize(IEnumerable<object> events, int aggregateVersion)
//        {
//            if (events == null) 
//                throw new ArgumentNullException(nameof(events));
//            
//            if (HasChanges())
//                throw new InvalidOperationException("Initialize cannot be called on an instance with changes.");
//            
//            foreach (var @event in events)
//            {
//                Play(@event);
//            }
//            
//            AggregateVersion = aggregateVersion;
//        }

        protected void ApplyChange(object @event)
        {
            if (@event == null) 
                throw new ArgumentNullException(nameof(@event));
            
            Play(@event);
            Record(@event);
        }

        private void Play(object @event)
        {
            if (@event == null) 
                throw new ArgumentNullException(nameof(@event));
            
            if (_eventHandlers.TryGetValue(@event.GetType(), out var handler))
            {
                handler(@event);
            }
        }

        private void Record(object @event) => _recordedEvents.Add(@event);

        public bool HasChanges() => _recordedEvents.Any();

        public List<object> GetChanges() => _recordedEvents.ToList();

        public void ClearChanges() => _recordedEvents.Clear();
    }
}