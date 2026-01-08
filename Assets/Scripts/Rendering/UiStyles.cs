using UnityEngine;

namespace MKGame.Rendering
{
    public static class UiStyles
    {
        private static GUIStyle _panel;
        private static GUIStyle _title;
        private static GUIStyle _label;
        private static Texture2D _panelTex;

        public static GUIStyle Panel
        {
            get
            {
                if (_panel == null)
                {
                    _panelTex = new Texture2D(1, 1);
                    _panelTex.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.45f));
                    _panelTex.Apply();
                    _panel = new GUIStyle(GUI.skin.box);
                    _panel.normal.background = _panelTex;
                    _panel.padding = new RectOffset(8, 8, 8, 8);
                }

                return _panel;
            }
        }

        public static GUIStyle Title
        {
            get
            {
                if (_title == null)
                {
                    _title = new GUIStyle(GUI.skin.label);
                    _title.fontSize = 14;
                    _title.fontStyle = FontStyle.Bold;
                    _title.normal.textColor = new Color(0.9f, 0.95f, 0.9f);
                }

                return _title;
            }
        }

        public static GUIStyle Label
        {
            get
            {
                if (_label == null)
                {
                    _label = new GUIStyle(GUI.skin.label);
                    _label.fontSize = 12;
                    _label.normal.textColor = new Color(0.85f, 0.9f, 0.85f);
                }

                return _label;
            }
        }
    }
}
