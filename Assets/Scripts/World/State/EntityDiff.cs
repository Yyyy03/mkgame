using UnityEngine;

namespace MKGame.World.State
{
    public struct EntityDiff
    {
        public EntityId Id;
        public Vector2Int OldPos;
        public Vector2Int NewPos;
    }
}