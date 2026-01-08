using System;
using System.Collections.Generic;

namespace MKGame.Events
{
    public static class EventTypeRegistry
    {
        private static readonly Dictionary<string, Type> _types = new Dictionary<string, Type>
        {
            { nameof(MoveEvent), typeof(MoveEvent) },
            { nameof(ResourcePickedEvent), typeof(ResourcePickedEvent) },
            { nameof(QuestCompletedEvent), typeof(QuestCompletedEvent) }
        };

        public static Type GetTypeByName(string typeName)
        {
            _types.TryGetValue(typeName, out var type);
            return type;
        }
    }
}
