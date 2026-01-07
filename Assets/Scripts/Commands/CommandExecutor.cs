using MKGame.World.State;
using MKGame.Events;

namespace MKGame.Commands
{
    public sealed class CommandExecutor : ICommandExecutor
    {
        private readonly IEventBus _eventBus;

        public CommandExecutor(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void ExecuteBatch(ICommandQueue queue, WorldState state, WorldDiff diff, int maxPerTick)
        {
            var executed = 0;
            while (executed < maxPerTick && queue.TryDequeue(out var cmd))
            {
                if (!cmd.CanExecute(state))
                {
                    executed++;
                    continue;
                }

                var result = cmd.Execute(state, diff);
                if (result != null && result.EmittedEvents.Count > 0)
                {
                    for (var i = 0; i < result.EmittedEvents.Count; i++)
                    {
                        _eventBus.Publish(result.EmittedEvents[i]);
                    }
                }

                executed++;
            }
        }
    }
}