using System.Collections.Generic;

namespace MKGame.World.State
{
    public sealed class MapState
    {
        public int Width;
        public int Height;
        public TileData[] Tiles;
        public BiomeData[] Biomes;
        public readonly List<ResourceNode> Resources = new List<ResourceNode>();
    }
}
