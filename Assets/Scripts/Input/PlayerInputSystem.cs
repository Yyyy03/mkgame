using UnityEngine;
using MKGame.AI.Decision;
using MKGame.Commands;
using MKGame.World.State;

namespace MKGame.Input
{
    public sealed class PlayerInputSystem : MonoBehaviour
    {
        public EntityId PlayerId = new EntityId(1);

        private readonly IntentTranslator _translator = new IntentTranslator();

        private void Update()
        {
            var delta = Vector2Int.zero;
            if (UnityEngine.Input.GetKeyDown(KeyCode.W)) delta = Vector2Int.up;
            if (UnityEngine.Input.GetKeyDown(KeyCode.S)) delta = Vector2Int.down;
            if (UnityEngine.Input.GetKeyDown(KeyCode.A)) delta = Vector2Int.left;
            if (UnityEngine.Input.GetKeyDown(KeyCode.D)) delta = Vector2Int.right;

            if (delta != Vector2Int.zero)
            {
                var intent = new Intent { Actor = PlayerId, TargetDelta = delta };
                var command = _translator.Translate(intent);
                MKGame.Core.GameRoot.Instance.CommandQueue.Enqueue(command);
            }
        }
    }
}