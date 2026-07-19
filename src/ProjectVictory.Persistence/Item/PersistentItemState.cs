using System;

namespace ProjectVictory.Persistence.Item;

public sealed class PersistentItemState
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}