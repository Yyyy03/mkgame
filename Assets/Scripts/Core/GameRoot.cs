using System;
using UnityEngine;
using MKGame.Core.Rng;
using MKGame.Core.Time;
using MKGame.World.State;
using MKGame.Events;
using MKGame.Commands;
using MKGame.AI.Decision;
using MKGame.Simulation;

namespace MKGame.Core
{
    // Scene entry point that wires pure systems together.
    public sealed class GameRoot : MonoBehaviour
    {
        public static GameRoot Instance { get; private set; }

        public WorldState WorldState { get; set; }
        public WorldDiff WorldDiff { get; private set; }
        public RngProvider RngProvider { get; private set; }
        public EventBus EventBus { get; private set; }
        public CommandQueue CommandQueue { get; private set; }
        public CommandExecutor CommandExecutor { get; private set; }
        public TickScheduler TickScheduler { get; private set; }
        public AiSystem AiSystem { get; private set; }
        public SimulationSystem SimulationSystem { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            WorldState = new WorldState();
            WorldDiff = new WorldDiff();
            RngProvider = new RngProvider(12345);
            EventBus = new EventBus(WorldState.Events);
            CommandQueue = new CommandQueue();
            CommandExecutor = new CommandExecutor(EventBus);
            TickScheduler = new TickScheduler();
            AiSystem = new AiSystem();
            SimulationSystem = new SimulationSystem();

            AiSystem.Initialize(WorldState, CommandQueue, RngProvider.Ai);
            SimulationSystem.Initialize(WorldState);
            TickScheduler.Register(AiSystem);
            TickScheduler.Register(SimulationSystem);
        }
    }
}
