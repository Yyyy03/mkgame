using MKGame.World.State;
using UnityEngine;

namespace MKGame.Events
{
    public sealed class MoveEvent : GameEvent
    {
        public EntityId Actor;
        public Vector2Int From;
        public Vector2Int To;

        public MoveEvent() { }

        public MoveEvent(EntityId actor, Vector2Int from, Vector2Int to)
        {
            Actor = actor;
            From = from;
            To = to;
        }
    }
}