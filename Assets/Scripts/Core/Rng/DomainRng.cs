using System;

namespace MKGame.Core.Rng
{
    public sealed class DomainRng : IRng
    {
        private System.Random _random;
        private RngState _state;

        public DomainRng(int seed)
        {
            _state = new RngState { Seed = seed, Calls = 0 };
            _random = new System.Random(seed);
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            _state.Calls++;
            return _random.Next(minInclusive, maxExclusive);
        }

        public float NextFloat()
        {
            _state.Calls++;
            return (float)_random.NextDouble();
        }

        public RngState GetState()
        {
            return _state;
        }

        public void SetState(RngState state)
        {
            _state = state;
            _random = new System.Random(state.Seed);
            for (var i = 0; i < state.Calls; i++)
            {
                _random.Next();
            }
        }
    }
}