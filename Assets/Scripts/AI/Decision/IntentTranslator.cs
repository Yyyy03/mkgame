using MKGame.Commands;

namespace MKGame.AI.Decision
{
    public sealed class IntentTranslator
    {
        public ICommand Translate(Intent intent)
        {
            return new MoveCommand(intent.Actor, intent.TargetDelta);
        }
    }
}