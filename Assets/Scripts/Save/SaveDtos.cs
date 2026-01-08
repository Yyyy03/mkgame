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
        public TaskStateDto Tasks;
        public RngStateBundle Rng;
        public long Tick;
    }

    public sealed class MapStateDto
    {
        public int Width;
        public int Height;
        public TileDto[] Tiles;
        public BiomeDto[] Biomes;
        public ResourceDto[] Resources;
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

    public struct ResourceDto
    {
        public int Id;
        public int Type;
        public int Amount;
        public int X;
        public int Y;
    }

    public sealed class TaskStateDto
    {
        public QuestDto[] Quests;
    }

    public struct QuestDto
    {
        public string Id;
        public string Title;
        public string Description;
        public int Type;
        public int Target;
        public int Progress;
        public bool Completed;
    }
}
