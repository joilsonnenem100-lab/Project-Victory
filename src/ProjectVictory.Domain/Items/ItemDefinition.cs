using System;

namespace ProjectVictory.Domain.Items;

public sealed class ItemDefinition
{
    public Guid Id { get; }

    public string Name { get; }

    public ItemCharacteristics Characteristics { get; }


    public ItemDefinition(
        Guid id,
        string name,
        ItemCharacteristics characteristics)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Item id is required.",
                nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Characteristics = characteristics ??
            throw new ArgumentNullException(nameof(characteristics));

        Id = id;
        Name = name;
    }
}