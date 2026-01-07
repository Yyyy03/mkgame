namespace MKGame.Core.Time
{
    public interface ITickable
    {
        TickDomain Domain { get; }
        void Tick(long tick, float deltaTime);
    }
}