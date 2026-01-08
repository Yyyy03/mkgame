namespace MKGame.Events
{
    public sealed class QuestCompletedEvent : GameEvent
    {
        public string QuestId;
        public string Title;

        public QuestCompletedEvent() { }

        public QuestCompletedEvent(string questId, string title)
        {
            QuestId = questId;
            Title = title;
        }
    }
}
