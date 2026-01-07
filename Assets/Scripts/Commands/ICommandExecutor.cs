using MKGame.World.State;

namespace MKGame.Commands
{
    public interface ICommandExecutor
    {
        void ExecuteBatch(ICommandQueue queue, WorldState state, WorldDiff diff, int maxPerTick);
    }
}