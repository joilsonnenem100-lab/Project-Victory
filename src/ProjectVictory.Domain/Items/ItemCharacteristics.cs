using System;

namespace ProjectVictory.Domain.Items;

public sealed class ItemCharacteristics
{
    public string Description { get; }


    public ItemCharacteristics(
        string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        Description = description;
    }
}