using System.Collections.Generic;
using UnityEngine;
using MKGame.World.State;

namespace MKGame.Events
{
    public sealed class EventLogUI : MonoBehaviour
    {
        public int MaxEntries = 8;

        private readonly List<string> _entries = new List<string>();
        private EventState _eventState;

        private void Start()
        {
            var root = MKGame.Core.GameRoot.Instance;
            _eventState = root.WorldState.Events;
            root.EventBus.Subscribe<MoveEvent>(OnMove);
            root.EventBus.Subscribe<ResourcePickedEvent>(OnResourcePicked);
            root.EventBus.Subscribe<QuestCompletedEvent>(OnQuestCompleted);
        }

        private void OnGUI()
        {
            var rootState = MKGame.Core.GameRoot.Instance.WorldState;
            if (!ReferenceEquals(_eventState, rootState.Events))
            {
                _eventState = rootState.Events;
            }

            var panel = new Rect(10, 10, 320, 210);
            GUI.Box(panel, string.Empty, MKGame.Rendering.UiStyles.Panel);
            var y = panel.y + 8;
            GUI.Label(new Rect(panel.x + 8, y, 300, 20), "Event Log", MKGame.Rendering.UiStyles.Title);
            y += 24;
            for (var i = 0; i < _entries.Count; i++)
            {
                GUI.Label(new Rect(panel.x + 8, y, 300, 18), _entries[i], MKGame.Rendering.UiStyles.Label);
                y += 16;
            }

            if (_eventState != null)
            {
                GUI.Label(new Rect(panel.x + 8, panel.y + panel.height - 22, 300, 18), "Queue: " + _eventState.EventQueue.Count, MKGame.Rendering.UiStyles.Label);
            }
        }

        private void OnMove(MoveEvent evt)
        {
            Push($"Move: {evt.Actor.Value} -> ({evt.To.x},{evt.To.y})");
        }

        private void OnResourcePicked(ResourcePickedEvent evt)
        {
            Push($"Pickup: {evt.Actor.Value} got {evt.Amount} at ({evt.X},{evt.Y})");
        }

        private void OnQuestCompleted(QuestCompletedEvent evt)
        {
            Push($"Quest: {evt.Title} completed");
        }

        private void Push(string msg)
        {
            _entries.Insert(0, msg);
            while (_entries.Count > MaxEntries)
            {
                _entries.RemoveAt(_entries.Count - 1);
            }
        }
    }
}
