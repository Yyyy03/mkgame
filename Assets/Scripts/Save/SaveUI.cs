using UnityEngine;

namespace MKGame.Save
{
    public sealed class SaveUI : MonoBehaviour
    {
        public string FileName = "save.json";

        private SaveDriver _saveDriver;
        private string _lastPath;
        private string _status;

        private void Start()
        {
            _saveDriver = GetComponent<SaveDriver>();
            if (_saveDriver == null)
            {
                _saveDriver = FindObjectOfType<SaveDriver>();
            }
        }

        private void OnGUI()
        {
            if (_saveDriver == null)
            {
                return;
            }

            const int width = 200;
            const int height = 28;
            const int padding = 10;
            var panel = new Rect(padding, padding + 240, width + 20, height * 3 + 18);
            var rectSave = new Rect(panel.x + 10, panel.y + 10, width, height);
            var rectLoad = new Rect(panel.x + 10, panel.y + 10 + height + 6, width, height);
            var rectLabel = new Rect(panel.x + 10, panel.y + 10 + (height + 6) * 2, 300, height);

            GUI.Box(panel, string.Empty, MKGame.Rendering.UiStyles.Panel);
            if (GUI.Button(rectSave, "Save to File"))
            {
                _lastPath = _saveDriver.SaveToFile(FileName);
                _status = "Saved: " + _lastPath;
            }

            if (GUI.Button(rectLoad, "Load from File"))
            {
                var path = _saveDriver.LoadFromFile(FileName);
                _status = path == null ? "No save file found." : "Loaded: " + path;
            }

            if (!string.IsNullOrEmpty(_status))
            {
                GUI.Label(rectLabel, _status, MKGame.Rendering.UiStyles.Label);
            }
        }
    }
}
