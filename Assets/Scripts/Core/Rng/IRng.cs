namespace MKGame.Core.Rng
{
    public interface IRng
    {
        int NextInt(int minInclusive, int maxExclusive);
        float NextFloat();
        RngState GetState();
        void SetState(RngState state);
    }
}