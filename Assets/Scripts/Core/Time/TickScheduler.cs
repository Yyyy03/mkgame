using System;
using System.Collections.Generic;

namespace MKGame.Core.Time
{
    public sealed class TickScheduler : ITickScheduler
    {
        public int LogicHz { get; private set; } = 10;
        public int AiHz { get; private set; } = 2;
        public int SimulationHz { get; private set; } = 1;
        public long CurrentTick { get; private set; }

        private readonly List<ITickable> _logicTickables = new List<ITickable>();
        private readonly List<ITickable> _aiTickables = new List<ITickable>();
        private readonly List<ITickable> _simTickables = new List<ITickable>();

        private float _accumulator;

        public void Register(ITickable tickable)
        {
            if (tickable == null) throw new ArgumentNullException(nameof(tickable));

            switch (tickable.Domain)
            {
                case TickDomain.Logic:
                    _logicTickables.Add(tickable);
                    break;
                case TickDomain.AI:
                    _aiTickables.Add(tickable);
                    break;
                case TickDomain.Simulation:
                    _simTickables.Add(tickable);
                    break;
            }
        }

        public void Tick(float deltaTime)
        {
            _accumulator += deltaTime;
            var logicDelta = 1f / LogicHz;

            while (_accumulator >= logicDelta)
            {
                _accumulator -= logicDelta;
                CurrentTick++;
                RunDomain(_logicTickables, CurrentTick, logicDelta);
                var aiStep = Math.Max(1, LogicHz / AiHz);
                if (CurrentTick % aiStep == 0)
                {
                    RunDomain(_aiTickables, CurrentTick, logicDelta);
                }
                var simStep = Math.Max(1, LogicHz / SimulationHz);
                if (CurrentTick % simStep == 0)
                {
                    RunDomain(_simTickables, CurrentTick, logicDelta);
                }
            }
        }

        private static void RunDomain(List<ITickable> list, long tick, float deltaTime)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list[i].Tick(tick, deltaTime);
            }
        }
    }
}
