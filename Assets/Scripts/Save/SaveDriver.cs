using System.IO;
using UnityEngine;

namespace MKGame.Save
{
    public sealed class SaveDriver : MonoBehaviour
    {
        private SaveSystem _saveSystem;
        public string DefaultFileName = "save.json";

        private void Start()
        {
            _saveSystem = new SaveSystem();
        }

        public string SaveToString()
        {
            var root = MKGame.Core.GameRoot.Instance;
            root.WorldState.Rng = root.RngProvider.GetState();
            return _saveSystem.Serialize(root.WorldState);
        }

        public void LoadFromString(string json)
        {
            var state = _saveSystem.Deserialize(json);
            var root = MKGame.Core.GameRoot.Instance;
            root.WorldState = state;
            root.EventBus.SetEventState(state.Events);
            root.RngProvider.SetState(state.Rng);
            root.AiSystem.Initialize(state, root.CommandQueue, root.RngProvider.Ai);
            root.SimulationSystem.Initialize(state);
            root.QuestSystem.Initialize(state, root.EventBus);
            root.QuestSystem.EnsureDefaultQuests();
        }

        public string SaveToFile(string fileName = null)
        {
            var name = string.IsNullOrEmpty(fileName) ? DefaultFileName : fileName;
            var path = Path.Combine(Application.persistentDataPath, name);
            var json = SaveToString();
            File.WriteAllText(path, json);
            return path;
        }

        public string LoadFromFile(string fileName = null)
        {
            var name = string.IsNullOrEmpty(fileName) ? DefaultFileName : fileName;
            var path = Path.Combine(Application.persistentDataPath, name);
            if (!File.Exists(path))
            {
                return null;
            }

            var json = File.ReadAllText(path);
            LoadFromString(json);
            return path;
        }
    }
}
