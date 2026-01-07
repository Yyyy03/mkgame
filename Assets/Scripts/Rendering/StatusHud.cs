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

            var label = $"Tick: {tick} | Entities: {entityCount} | Events: {eventCount}";
            GUI.Label(new Rect(10, Screen.height - 30, 600, 25), label);
        }
    }
}
