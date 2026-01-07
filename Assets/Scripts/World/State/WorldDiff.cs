using System.Collections.Generic;

namespace MKGame.World.State
{
    public sealed class WorldDiff
    {
        public readonly List<EntityDiff> EntityDiffs = new List<EntityDiff>();
        public readonly List<TileDiff> TileDiffs = new List<TileDiff>();

        public void Clear()
        {
            EntityDiffs.Clear();
            TileDiffs.Clear();
        }
    }
}