using UnityEngine;
using MKGame.World.State;

namespace MKGame.Rendering
{
    public sealed class CameraFollow : MonoBehaviour
    {
        public int PlayerEntityId = 1;
        public float SmoothTime = 0.12f;
        public bool PixelSnap = true;
        public int PixelsPerUnit = 16;

        private Vector3 _velocity;
        private WorldState _state;

        private void Start()
        {
            _state = MKGame.Core.GameRoot.Instance.WorldState;
        }

        private void LateUpdate()
        {
            var rootState = MKGame.Core.GameRoot.Instance.WorldState;
            if (!ReferenceEquals(_state, rootState))
            {
                _state = rootState;
            }

            var targetPos = new Vector3(0f, 0f, transform.position.z);
            var id = new EntityId(PlayerEntityId);
            if (_state.Entities.Entities.TryGetValue(id, out var data))
            {
                targetPos = new Vector3(data.Position.x, data.Position.y, transform.position.z);
            }

            var smooth = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, SmoothTime);
            if (PixelSnap)
            {
                var unit = 1f / Mathf.Max(1, PixelsPerUnit);
                smooth.x = Mathf.Round(smooth.x / unit) * unit;
                smooth.y = Mathf.Round(smooth.y / unit) * unit;
            }

            transform.position = smooth;
        }
    }
}
