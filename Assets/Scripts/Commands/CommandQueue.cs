using System.Collections.Generic;

namespace MKGame.Commands
{
    public sealed class CommandQueue : ICommandQueue
    {
        private readonly Queue<ICommand> _queue = new Queue<ICommand>();

        public int Count => _queue.Count;

        public void Enqueue(ICommand command)
        {
            _queue.Enqueue(command);
        }

        public bool TryDequeue(out ICommand command)
        {
            if (_queue.Count > 0)
            {
                command = _queue.Dequeue();
                return true;
            }

            command = null;
            return false;
        }
    }
}