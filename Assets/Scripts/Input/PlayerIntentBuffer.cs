using System.Collections.Generic;
using MKGame.AI.Decision;

namespace MKGame.Input
{
    public sealed class PlayerIntentBuffer
    {
        private readonly Queue<Intent> _queue = new Queue<Intent>();

        public void Enqueue(Intent intent)
        {
            _queue.Enqueue(intent);
        }

        public bool TryDequeue(out Intent intent)
        {
            if (_queue.Count > 0)
            {
                intent = _queue.Dequeue();
                return true;
            }

            intent = null;
            return false;
        }
    }
}