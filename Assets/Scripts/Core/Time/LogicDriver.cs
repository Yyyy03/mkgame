using UnityEngine;
using MKGame.Commands;
using MKGame.Events;
using MKGame.World.State;

namespace MKGame.Core.Time
{
    public sealed class LogicDriver : MonoBehaviour, ITickable
    {
        public int MaxCommandsPerTick = 32;

        private TickScheduler _scheduler;
        public TickDomain Domain => TickDomain.Logic;

        private void Start()
        {
            _scheduler = MKGame.Core.GameRoot.Instance.TickScheduler;
            _scheduler.Register(this);
        }

        public void Tick(long tick, float deltaTime)
        {
            var root = MKGame.Core.GameRoot.Instance;
            root.CommandExecutor.ExecuteBatch(root.CommandQueue, root.WorldState, root.WorldDiff, MaxCommandsPerTick);
            root.EventBus.ProcessQueue();
            root.WorldState.Tick = tick;
            root.WorldState.Rng = root.RngProvider.GetState();
        }
    }
}
