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

    // PV-PROG-010: cache do Level derivado (PV-GOV-006 D2; level.md). Gravado, nunca lido como verdade.
    public int Level { get; set; }

    public long Experience { get; set; }

    // PV-REWARD-010: ouro do Inventory (Resource, ADR-035).
    public long Gold { get; set; }

    public List<PersistentItemInstanceState> InventoryItems { get; set; } = new();

    // PV-CHAR-013 (PO-37/PO-39, colunas aditivas): aparência e exclusão (instantes em ms Unix).
    // Gender 0 = não informado (personagens anteriores). DeletedAt preenchido = excluído (some da conta).
    public int Gender { get; set; }

    public string? Outfit { get; set; }

    public long? DeletionRequestedAtUnixMilliseconds { get; set; }

    public long? DeletionScheduledForUnixMilliseconds { get; set; }

    public long? DeletedAtUnixMilliseconds { get; set; }
}