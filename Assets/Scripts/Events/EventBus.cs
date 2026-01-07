using System;
using System.Collections.Generic;
using MKGame.World.State;

namespace MKGame.Events
{
    public sealed class EventBus : IEventBus
    {
        private EventState _eventState;
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public EventBus(EventState eventState)
        {
            _eventState = eventState;
        }

        public void SetEventState(EventState eventState)
        {
            _eventState = eventState;
        }

        public void Publish(GameEvent evt)
        {
            _eventState.EventQueue.Enqueue(evt);
        }

        public void Subscribe<T>(Action<T> handler) where T : GameEvent
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _subscribers[type] = list;
            }

            list.Add(handler);
        }

        public void ProcessQueue()
        {
            while (_eventState.EventQueue.Count > 0)
            {
                var evt = _eventState.EventQueue.Dequeue();
                var type = evt.GetType();
                if (_subscribers.TryGetValue(type, out var list))
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        list[i].DynamicInvoke(evt);
                    }
                }
            }
        }
    }
}
