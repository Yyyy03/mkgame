using System.Collections.Generic;
using MKGame.Core.Rng;
using MKGame.Events;

namespace MKGame.Save
{
    public sealed class WorldStateDto
    {
        public MapStateDto Map;
        public EntityStateDto Entities;
        public SimulationStateDto Simulation;
        public EventStateDto Events;
        public RngStateBundle Rng;
        public long Tick;
    }

    public sealed class MapStateDto
    {
        public int Width;
        public int Height;
        public TileDto[] Tiles;
        public BiomeDto[] Biomes;
    }

    public sealed class EntityStateDto
    {
        public List<EntityDto> Entities;
    }

    public sealed class SimulationStateDto
    {
        public float TimeOfDay;
        public int SeasonIndex;
        public int EcoPopulation;
    }

    public sealed class EventStateDto
    {
        public List<EventDto> Queue;
    }

    public struct EntityDto
    {
        public int Id;
        public int X;
        public int Y;
        public int Hp;
        public int Energy;
        public int Faction;
    }

    public struct TileDto
    {
        public int TileType;
        public byte Flags;
    }

    public struct BiomeDto
    {
        public int BiomeType;
    }
}