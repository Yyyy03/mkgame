using UnityEngine;
using MKGame.Core.Rng;
using MKGame.World.State;

namespace MKGame.World.Generation
{
    public sealed class WorldGenerator
    {
        public void Generate(WorldState state, DomainRng rng, int width, int height, int npcCount)
        {
            state.Map.Width = width;
            state.Map.Height = height;
            state.Map.Tiles = new TileData[width * height];
            state.Map.Biomes = new BiomeData[width * height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = (y * width) + x;
                    var tileType = (x + y + rng.NextInt(0, 2)) % 2;
                    state.Map.Tiles[index] = new TileData { TileType = tileType, Flags = 0 };
                    state.Map.Biomes[index] = new BiomeData { BiomeType = tileType };
                }
            }

            state.Entities.Entities.Clear();
            var playerId = new EntityId(1);
            state.Entities.Entities[playerId] = new EntityData
            {
                Id = playerId,
                Position = new Vector2Int(width / 2, height / 2),
                Hp = 100,
                Energy = 100,
                Faction = 0
            };

            for (var i = 0; i < npcCount; i++)
            {
                var id = new EntityId(2 + i);
                var pos = new Vector2Int(rng.NextInt(0, width), rng.NextInt(0, height));
                state.Entities.Entities[id] = new EntityData
                {
                    Id = id,
                    Position = pos,
                    Hp = 50,
                    Energy = 50,
                    Faction = 1
                };
            }
        }
    }
}
