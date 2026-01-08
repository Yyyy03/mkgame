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
        }

        private void OnGUI()
        {
            var rootState = MKGame.Core.GameRoot.Instance.WorldState;
            if (!ReferenceEquals(_eventState, rootState.Events))
            {
                _eventState = rootState.Events;
            }

            var y = 10;
            GUI.Label(new Rect(10, y, 300, 20), "Event Log");
            y += 22;
            for (var i = 0; i < _entries.Count; i++)
            {
                GUI.Label(new Rect(10, y, 600, 20), _entries[i]);
                y += 18;
            }

            if (_eventState != null)
            {
                GUI.Label(new Rect(10, y + 6, 300, 20), "Queue: " + _eventState.EventQueue.Count);
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
