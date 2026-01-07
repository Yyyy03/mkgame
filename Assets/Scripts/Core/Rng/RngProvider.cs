namespace MKGame.Core.Rng
{
    public sealed class RngProvider
    {
        public DomainRng WorldGen { get; }
        public DomainRng Simulation { get; }
        public DomainRng Ai { get; }
        public DomainRng Events { get; }

        public RngProvider(int seed)
        {
            WorldGen = new DomainRng(seed + 1);
            Simulation = new DomainRng(seed + 2);
            Ai = new DomainRng(seed + 3);
            Events = new DomainRng(seed + 4);
        }

        public RngStateBundle GetState()
        {
            return new RngStateBundle
            {
                WorldGen = WorldGen.GetState(),
                Simulation = Simulation.GetState(),
                Ai = Ai.GetState(),
                Events = Events.GetState()
            };
        }

        public void SetState(RngStateBundle bundle)
        {
            WorldGen.SetState(bundle.WorldGen);
            Simulation.SetState(bundle.Simulation);
            Ai.SetState(bundle.Ai);
            Events.SetState(bundle.Events);
        }
    }
}