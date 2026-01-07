using System.Collections.Generic;
using MKGame.Events;

namespace MKGame.World.State
{
    public sealed class EventState
    {
        // The single authoritative event queue for save/load.
        public readonly Queue<GameEvent> EventQueue = new Queue<GameEvent>();
    }
}