using System;

namespace ProjectVictory.Persistence.Character;

public sealed class PersistentItemInstanceState
{
    public Guid Id { get; set; }

    public Guid ItemDefinitionId { get; set; }
}