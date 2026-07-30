namespace ProjectVictory.Persistence.Runtime;

public sealed class PersistentRuntimeState
{
    public Guid Id { get; set; }

    public Guid RuntimeId { get; set; }

    public Guid CharacterId { get; set; }

    public bool Initialized { get; set; }

    public int Health { get; set; }

    public int Mana { get; set; }
}
