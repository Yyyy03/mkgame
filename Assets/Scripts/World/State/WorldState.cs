using MKGame.Core.Rng;

namespace MKGame.World.State
{
    public sealed class WorldState
    {
        public readonly MapState Map = new MapState();
        public readonly EntityState Entities = new EntityState();
        public readonly SimulationState Simulation = new SimulationState();
        public readonly EventState Events = new EventState();
        public readonly TaskState Tasks = new TaskState();
        public RngStateBundle Rng;
        public long Tick;
    }
}
