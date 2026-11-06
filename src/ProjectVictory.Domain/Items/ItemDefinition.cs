using System;

namespace ProjectVictory.Domain.Items;

public sealed class ItemDefinition
{
    public Guid Id { get; }

    public string Name { get; }

    public ItemCharacteristics Characteristics { get; }

    // PV-REWARD-010: preço de venda ao NPC (DEC-20261106-08). Valores PROVISÓRIOS do
    // Arquiteto, ajustáveis pelo PO. É dado do item; comércio com NPC é Fase 7/8.
    public int NpcSellPrice { get; }


    public ItemDefinition(
        Guid id,
        string name,
        ItemCharacteristics characteristics)
        : this(id, name, characteristics, 0)
    {
    }


    public ItemDefinition(
        Guid id,
        string name,
        ItemCharacteristics characteristics,
        int npcSellPrice)
    {
        if (npcSellPrice < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(npcSellPrice),
                "NPC sell price cannot be negative.");
        }

        NpcSellPrice = npcSellPrice;

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