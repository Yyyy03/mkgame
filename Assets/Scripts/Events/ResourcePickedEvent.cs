using MKGame.World.State;

namespace MKGame.Events
{
    public sealed class ResourcePickedEvent : GameEvent
    {
        public EntityId Actor;
        public int ResourceId;
        public int ResourceType;
        public int Amount;
        public int X;
        public int Y;

        public ResourcePickedEvent() { }

        public ResourcePickedEvent(EntityId actor, ResourceNode node)
        {
            Actor = actor;
            ResourceId = node.Id;
            ResourceType = node.Type;
            Amount = node.Amount;
            X = node.Position.x;
            Y = node.Position.y;
        }
    }
}
