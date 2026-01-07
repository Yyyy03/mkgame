using UnityEngine;
using MKGame.World.Generation;

namespace MKGame.World.Generation
{
    public sealed class WorldGenDriver : MonoBehaviour
    {
        public int Width = 64;
        public int Height = 64;
        public int NpcCount = 10;

        private WorldGenerator _generator;

        private void Start()
        {
            _generator = new WorldGenerator();
            var root = MKGame.Core.GameRoot.Instance;
            _generator.Generate(root.WorldState, root.RngProvider.WorldGen, Width, Height, NpcCount);
            root.WorldState.Rng = root.RngProvider.GetState();
        }
    }
}
