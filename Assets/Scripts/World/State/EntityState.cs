using System.Collections.Generic;

namespace MKGame.World.State
{
    public sealed class EntityState
    {
        public readonly Dictionary<EntityId, EntityData> Entities = new Dictionary<EntityId, EntityData>();
    }
}