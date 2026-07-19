using System;

namespace ProjectVictory.Persistence.Monster;

public sealed class PersistentMonsterState
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Health { get; set; }

    public string State { get; set; } = string.Empty;
}