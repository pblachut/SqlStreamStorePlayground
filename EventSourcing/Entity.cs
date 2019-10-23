using System;
using System.Collections.Generic;

namespace EventSourcing
{
    public abstract class Entity
    {
        private readonly Action<object> _applier;
        private readonly Dictionary<Type, Action<object>> _eventHandlers;

        public long EntityId { get; protected set; }

        protected Entity(Action<object> applier)
        {
            _applier = applier ?? throw new ArgumentNullException(nameof (applier));
            _eventHandlers = new Dictionary<Type, Action<object>>();
        }

        protected void On<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) 
                throw new ArgumentNullException(nameof(handler));

            _eventHandlers.Add(typeof (TEvent), @event => handler((TEvent) @event));
        }

        public void Route(object @event)
        {
            if (@event == null) 
                throw new ArgumentNullException(nameof(@event));
            
            if (_eventHandlers.TryGetValue(@event.GetType(), out var handler))
            {
                handler(@event);
            }
        }

        protected void Apply(object @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof (@event));
            
            _applier(@event);
        }
    }
}