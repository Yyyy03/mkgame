using UnityEngine;

namespace MKGame.World.State
{
    public struct EntityData
    {
        public EntityId Id;
        public Vector2Int Position;
        public int Hp;
        public int Energy;
        public int Faction;
    }
}