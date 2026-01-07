using MKGame.World.State;
using MKGame.Events;
using UnityEngine;

namespace MKGame.Commands
{
    public sealed class MoveCommand : ICommand
    {
        public EntityId Actor { get; private set; }
        public Vector2Int Delta { get; private set; }

        public MoveCommand(EntityId actor, Vector2Int delta)
        {
            Actor = actor;
            Delta = delta;
        }

        public bool CanExecute(WorldState state)
        {
            // Pure read check only.
            if (!state.Entities.Entities.TryGetValue(Actor, out var data))
            {
                return false;
            }

            var map = state.Map;
            if (map.Width <= 0 || map.Height <= 0)
            {
                return false;
            }

            var next = data.Position + Delta;
            return next.x >= 0 && next.x < map.Width && next.y >= 0 && next.y < map.Height;
        }

        public CommandResult Execute(WorldState state, WorldDiff diff)
        {
            var result = new CommandResult { Success = true };
            var data = state.Entities.Entities[Actor];
            var oldPos = data.Position;
            data.Position += Delta;
            state.Entities.Entities[Actor] = data;
            diff.EntityDiffs.Add(new EntityDiff { Id = Actor, OldPos = oldPos, NewPos = data.Position });
            result.EmittedEvents.Add(new MoveEvent(Actor, oldPos, data.Position));
            return result;
        }
    }
}
