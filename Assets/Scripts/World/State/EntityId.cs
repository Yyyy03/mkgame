namespace MKGame.World.State
{
    public struct EntityId
    {
        public int Value;
        public EntityId(int value) { Value = value; }
        public override string ToString() => Value.ToString();
    }
}