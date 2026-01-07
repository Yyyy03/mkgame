using System.Collections.Generic;
using UnityEngine;
using MKGame.Core.Rng;
using MKGame.Core.Time;
using MKGame.Commands;
using MKGame.World.State;

namespace MKGame.AI.Decision
{
    public sealed class AiSystem : IAiSystem
    {
        private WorldState _state;
        private ICommandQueue _queue;
        private DomainRng _rng;
        private readonly List<EntityId> _npcIds = new List<EntityId>();

        public TickDomain Domain => TickDomain.AI;

        public void Initialize(WorldState state, ICommandQueue queue, DomainRng rng)
        {
            _state = state;
            _queue = queue;
            _rng = rng;
        }

        public void Tick(long tick, float deltaTime)
        {
            if (_state == null || _queue == null || _rng == null)
            {
                return;
            }

            _npcIds.Clear();
            foreach (var kv in _state.Entities.Entities)
            {
                if (kv.Key.Value == 1)
                {
                    continue;
                }

                _npcIds.Add(kv.Key);
            }

            if (_npcIds.Count == 0)
            {
                return;
            }

            var map = _state.Map;
            for (var i = 0; i < _npcIds.Count; i++)
            {
                var id = _npcIds[i];
                var data = _state.Entities.Entities[id];
                var delta = RandomDelta();
                var next = data.Position + delta;
                if (next.x < 0 || next.x >= map.Width || next.y < 0 || next.y >= map.Height)
                {
                    continue;
                }

                _queue.Enqueue(new MoveCommand(id, delta));
            }
        }

        private Vector2Int RandomDelta()
        {
            var roll = _rng.NextInt(0, 4);
            switch (roll)
            {
                case 0: return Vector2Int.up;
                case 1: return Vector2Int.down;
                case 2: return Vector2Int.left;
                default: return Vector2Int.right;
            }
        }
    }
}
