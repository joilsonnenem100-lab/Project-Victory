namespace ProjectVictory.Domain.Entities
{
    public sealed class Attributes
    {
        public int Strength { get; private set; } = 0;

        public int Dexterity { get; private set; } = 0;

        public int Intelligence { get; private set; } = 0;

        public int Vitality { get; private set; } = 0;
    }
}