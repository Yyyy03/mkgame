using MKGame.World.State;

namespace MKGame.Commands
{
    public interface ICommand
    {
        EntityId Actor { get; }
        bool CanExecute(WorldState state);
        CommandResult Execute(WorldState state, WorldDiff diff);
    }
}