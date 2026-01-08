using UnityEngine;

namespace MKGame.Rendering
{
    public sealed class StatusHud : MonoBehaviour
    {
        private MKGame.Core.GameRoot _root;

        private void Start()
        {
            _root = MKGame.Core.GameRoot.Instance;
        }

        private void OnGUI()
        {
            if (_root == null)
            {
                return;
            }

            var state = _root.WorldState;
            var entityCount = state.Entities.Entities.Count;
            var eventCount = state.Events.EventQueue.Count;
            var tick = state.Tick;

            var time = state.Simulation.TimeOfDay;
            var timeLabel = $"Time: {time:00.0}h";
            var label = $"Tick: {tick} | Entities: {entityCount} | Events: {eventCount} | {timeLabel}";
            var rect = new Rect(10, Screen.height - 40, 620, 30);
            GUI.Box(rect, string.Empty, UiStyles.Panel);
            GUI.Label(new Rect(rect.x + 8, rect.y + 6, rect.width - 16, rect.height), label, UiStyles.Label);
        }
    }
}
