using MKGame.World.State;
using MKGame.Events;

namespace MKGame.Commands
{
    public sealed class PickupResourceCommand : ICommand
    {
        public EntityId Actor { get; private set; }

        public PickupResourceCommand(EntityId actor)
        {
            Actor = actor;
        }

        public bool CanExecute(WorldState state)
        {
            if (!state.Entities.Entities.TryGetValue(Actor, out var data))
            {
                return false;
            }

            var pos = data.Position;
            var resources = state.Map.Resources;
            for (var i = 0; i < resources.Count; i++)
            {
                if (resources[i].Position == pos)
                {
                    return true;
                }
            }

            return false;
        }

        public CommandResult Execute(WorldState state, WorldDiff diff)
        {
            var result = new CommandResult { Success = false };
            if (!state.Entities.Entities.TryGetValue(Actor, out var data))
            {
                result.FailureReason = "Actor missing.";
                return result;
            }

            var pos = data.Position;
            var resources = state.Map.Resources;
            for (var i = 0; i < resources.Count; i++)
            {
                if (resources[i].Position == pos)
                {
                    var node = resources[i];
                    resources.RemoveAt(i);
                    data.Energy = ClampEnergy(data.Energy + (node.Amount * 5));
                    state.Entities.Entities[Actor] = data;
                    result.Success = true;
                    result.EmittedEvents.Add(new ResourcePickedEvent(Actor, node));
                    return result;
                }
            }

            result.FailureReason = "No resource here.";
            return result;
        }

        private static int ClampEnergy(int energy)
        {
            if (energy < 0) return 0;
            if (energy > 100) return 100;
            return energy;
        }
    }
}
