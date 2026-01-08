using UnityEngine;
using MKGame.World.State;
using MKGame.Rendering;

namespace MKGame.Tasks
{
    public sealed class QuestUI : MonoBehaviour
    {
        public int MaxVisible = 6;

        private WorldState _state;

        private void Start()
        {
            _state = MKGame.Core.GameRoot.Instance.WorldState;
        }

        private void OnGUI()
        {
            var rootState = MKGame.Core.GameRoot.Instance.WorldState;
            if (!ReferenceEquals(_state, rootState))
            {
                _state = rootState;
            }

            var panel = new Rect(Screen.width - 330, 10, 320, 220);
            GUI.Box(panel, string.Empty, UiStyles.Panel);
            var y = panel.y + 8;
            GUI.Label(new Rect(panel.x + 8, y, 300, 20), "Quests", UiStyles.Title);
            y += 24;

            var list = _state.Tasks.Quests;
            var count = Mathf.Min(MaxVisible, list.Count);
            for (var i = 0; i < count; i++)
            {
                var q = list[i];
                var status = q.Completed ? "Done" : $"{q.Progress}/{q.Target}";
                var line = $"{q.Title} - {status}";
                GUI.Label(new Rect(panel.x + 8, y, 300, 18), line, UiStyles.Label);
                y += 16;
            }
        }
    }
}
