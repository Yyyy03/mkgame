using System.Collections.Generic;

namespace MKGame.World.State
{
    public sealed class TaskState
    {
        public readonly List<QuestState> Quests = new List<QuestState>();
    }
}
