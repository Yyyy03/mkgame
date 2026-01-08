using MKGame.Events;
using MKGame.World.State;

namespace MKGame.Tasks
{
    public sealed class QuestSystem
    {
        private WorldState _state;
        private IEventBus _eventBus;
        private bool _subscribed;

        public void Initialize(WorldState state, IEventBus eventBus)
        {
            _state = state;
            _eventBus = eventBus;
            if (!_subscribed)
            {
                _eventBus.Subscribe<MoveEvent>(OnMove);
                _eventBus.Subscribe<ResourcePickedEvent>(OnResourcePicked);
                _subscribed = true;
            }
        }

        public void EnsureDefaultQuests()
        {
            if (_state == null || _state.Tasks.Quests.Count > 0)
            {
                return;
            }

            _state.Tasks.Quests.Add(new QuestState
            {
                Id = "move_10",
                Title = "Wanderer",
                Description = "Move 10 steps.",
                Type = QuestType.MoveSteps,
                Target = 10,
                Progress = 0,
                Completed = false
            });

            _state.Tasks.Quests.Add(new QuestState
            {
                Id = "gather_3",
                Title = "Forager",
                Description = "Pick up 3 resources.",
                Type = QuestType.PickupResources,
                Target = 3,
                Progress = 0,
                Completed = false
            });
        }

        private void OnMove(MoveEvent evt)
        {
            if (_state == null)
            {
                return;
            }

            if (evt.Actor.Value != 1)
            {
                return;
            }

            var list = _state.Tasks.Quests;
            for (var i = 0; i < list.Count; i++)
            {
                var q = list[i];
                if (q.Completed || q.Type != QuestType.MoveSteps)
                {
                    continue;
                }

                q.Progress++;
                if (q.Progress >= q.Target)
                {
                    q.Completed = true;
                    _eventBus.Publish(new QuestCompletedEvent(q.Id, q.Title));
                }
            }
        }

        private void OnResourcePicked(ResourcePickedEvent evt)
        {
            if (_state == null)
            {
                return;
            }

            if (evt.Actor.Value != 1)
            {
                return;
            }

            var list = _state.Tasks.Quests;
            for (var i = 0; i < list.Count; i++)
            {
                var q = list[i];
                if (q.Completed || q.Type != QuestType.PickupResources)
                {
                    continue;
                }

                q.Progress++;
                if (q.Progress >= q.Target)
                {
                    q.Completed = true;
                    _eventBus.Publish(new QuestCompletedEvent(q.Id, q.Title));
                }
            }
        }
    }
}
