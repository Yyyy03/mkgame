namespace MKGame.Commands
{
    public interface ICommandQueue
    {
        void Enqueue(ICommand command);
        bool TryDequeue(out ICommand command);
        int Count { get; }
    }
}