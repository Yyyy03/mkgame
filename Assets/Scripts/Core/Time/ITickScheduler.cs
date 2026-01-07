namespace MKGame.Core.Time
{
    public interface ITickScheduler
    {
        int LogicHz { get; }
        int AiHz { get; }
        int SimulationHz { get; }
        long CurrentTick { get; }

        void Register(ITickable tickable);
        void Tick(float deltaTime);
    }
}