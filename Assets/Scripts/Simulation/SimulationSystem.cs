using MKGame.Core.Time;
using MKGame.World.State;

namespace MKGame.Simulation
{
    public sealed class SimulationSystem : ISimulationSystem
    {
        private const int MaxEnergy = 100;
        private WorldState _state;

        public TickDomain Domain => TickDomain.Simulation;

        public void Initialize(WorldState state)
        {
            _state = state;
        }

        public void Tick(long tick, float deltaTime)
        {
            if (_state == null)
            {
                return;
            }

            // Day/Night cycle: TimeOfDay in hours [0,24).
            _state.Simulation.TimeOfDay += 1f;
            if (_state.Simulation.TimeOfDay >= 24f)
            {
                _state.Simulation.TimeOfDay -= 24f;
                _state.Simulation.SeasonIndex = (_state.Simulation.SeasonIndex + 1) % 4;
            }

            var isNight = _state.Simulation.TimeOfDay >= 18f || _state.Simulation.TimeOfDay < 6f;

            var keys = new System.Collections.Generic.List<EntityId>(_state.Entities.Entities.Keys);
            for (var i = 0; i < keys.Count; i++)
            {
                var id = keys[i];
                var data = _state.Entities.Entities[id];
                if (isNight)
                {
                    data.Energy = ClampEnergy(data.Energy + 1);
                }
                else
                {
                    data.Energy = ClampEnergy(data.Energy - 1);
                }

                _state.Entities.Entities[id] = data;
            }
        }

        private static int ClampEnergy(int value)
        {
            if (value < 0) return 0;
            if (value > MaxEnergy) return MaxEnergy;
            return value;
        }
    }
}
