using System.Text.Json;

namespace MKGame.Events
{
    public sealed class EventDto
    {
        public string Type;
        public JsonElement Payload;
    }
}