using System.Collections.Generic;
using MKGame.Events;

namespace MKGame.Commands
{
    public sealed class CommandResult
    {
        public bool Success;
        public readonly List<GameEvent> EmittedEvents = new List<GameEvent>();
        public string FailureReason;
    }
}