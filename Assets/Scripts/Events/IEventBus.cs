using System;

namespace MKGame.Events
{
    public interface IEventBus
    {
        void Publish(GameEvent evt);
        void Subscribe<T>(Action<T> handler) where T : GameEvent;
        void ProcessQueue();
    }
}