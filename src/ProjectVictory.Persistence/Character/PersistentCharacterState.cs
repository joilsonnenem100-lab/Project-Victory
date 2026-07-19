using System;

namespace ProjectVictory.Persistence.Character;

public sealed class PersistentCharacterState
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Level { get; set; }

    public int Experience { get; set; }
}