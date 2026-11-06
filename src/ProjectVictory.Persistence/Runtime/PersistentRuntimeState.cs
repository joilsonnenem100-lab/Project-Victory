namespace ProjectVictory.Persistence.Runtime;

public sealed class PersistentRuntimeState
{
    public Guid Id { get; set; }

    public Guid RuntimeId { get; set; }

    public Guid CharacterId { get; set; }

    public bool Initialized { get; set; }

    public int Health { get; set; }

    public int Mana { get; set; }

    // PV-RUNTIME-010 (ADR-057 v1.2): última Hunt escolhida — checkpoint mínimo do Runtime State.
    public string? LastHuntArea { get; set; }
}
