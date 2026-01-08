namespace MKGame.World.State
{
    public enum QuestType
    {
        MoveSteps,
        PickupResources
    }

    public sealed class QuestState
    {
        public string Id;
        public string Title;
        public string Description;
        public QuestType Type;
        public int Target;
        public int Progress;
        public bool Completed;
    }
}
