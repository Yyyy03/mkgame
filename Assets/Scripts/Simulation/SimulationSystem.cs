using MKGame.Core.Time;

namespace MKGame.Simulation
{
    public sealed class SimulationSystem : ISimulationSystem
    {
        public TickDomain Domain => TickDomain.Simulation;

        public void Tick(long tick, float deltaTime)
        {
            // Simulation tick placeholder.
        }
    }
}