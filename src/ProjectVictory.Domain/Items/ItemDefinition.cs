using System;

namespace ProjectVictory.Domain.Items;

public sealed class ItemDefinition
{
    public Guid Id { get; }

    public string Name { get; }


    public ItemDefinition(
        Guid id,
        string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Item id is required.",
                nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Id = id;
        Name = name;
    }
}