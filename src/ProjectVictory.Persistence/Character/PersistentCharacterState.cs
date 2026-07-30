// src/ProjectVictory.Persistence/Character/PersistentCharacterState.cs

using System;
using System.Collections.Generic;

namespace ProjectVictory.Persistence.Character;

public sealed class PersistentCharacterState
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CharacterClass { get; set; }

    public int Health { get; set; }

    public int Mana { get; set; }

    public int Level { get; set; }

    public int Experience { get; set; }

    public List<PersistentItemInstanceState> InventoryItems { get; set; } = new();
}