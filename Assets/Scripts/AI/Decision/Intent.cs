using UnityEngine;
using MKGame.World.State;

namespace MKGame.AI.Decision
{
    public sealed class Intent
    {
        public EntityId Actor;
        public Vector2Int TargetDelta;
    }
}