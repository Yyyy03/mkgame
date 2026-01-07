using UnityEngine;

namespace MKGame.Save
{
    public sealed class SaveHotkeys : MonoBehaviour
    {
        public KeyCode SaveKey = KeyCode.F5;
        public KeyCode LoadKey = KeyCode.F9;

        private SaveDriver _saveDriver;
        private string _lastSave;

        private void Start()
        {
            _saveDriver = GetComponent<SaveDriver>();
            if (_saveDriver == null)
            {
                _saveDriver = FindObjectOfType<SaveDriver>();
            }
        }

        private void Update()
        {
            if (_saveDriver == null)
            {
                return;
            }

            if (UnityEngine.Input.GetKeyDown(SaveKey))
            {
                _lastSave = _saveDriver.SaveToString();
                Debug.Log("Saved snapshot (in memory).");
            }

            if (UnityEngine.Input.GetKeyDown(LoadKey))
            {
                if (!string.IsNullOrEmpty(_lastSave))
                {
                    _saveDriver.LoadFromString(_lastSave);
                    Debug.Log("Loaded snapshot (from memory).");
                }
                else
                {
                    Debug.LogWarning("No saved snapshot in memory.");
                }
            }
        }
    }
}
