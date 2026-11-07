using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Combat.TurnBased;

public enum CombatSide
{
    Player,
    Monster
}

// PV-COMBAT-010/012: atributos de combate usados pelas regras de dano (IDamageFormula).
// Os valores vêm de tabelas de dados PROVISÓRIAS (classe do jogador, definição do monstro).
public sealed record CombatProfile(
    int AttackSkill,
    int WeaponAttack,
    int Armor,
    int Defense,
    int AbilityManaCost = 0,
    decimal AbilityDamageMultiplier = 1m,
    // PV-COMBAT-013: força do ataque básico (Knight forte, Paladin muito forte), em dados.
    decimal BasicAttackMultiplier = 1m);

// PV-COMBAT-010: participante de um combate por turnos (jogador ou monstro).
// Guarda só estado; nenhuma regra de dano, IA, loot ou XP (ADR-051).
public sealed class Combatant
{
    public Guid Id { get; }

    // CharacterId do jogador ou MonsterId da definição do monstro.
    public Guid SourceId { get; }

    public CombatSide Side { get; }

    public string Name { get; }

    public int Level { get; }

    public int MaximumHealth { get; }

    public int CurrentHealth { get; private set; }

    public int MaximumMana { get; }

    public int CurrentMana { get; private set; }

    public CombatProfile Profile { get; }

    // PV-COMBAT-013: skills disponíveis (já desbloqueadas pelo Level); vazio para monstros.
    public IReadOnlyList<ProjectVictory.Domain.Combat.Rules.SkillDefinition> Skills { get; }

    // PV-COMBAT-014 (PO-53): spell de cura da vocação (dados); null para monstros.
    public ProjectVictory.Domain.Combat.Rules.HealingSpellDefinition? HealingSpell { get; }

    public bool IsDead => CurrentHealth <= 0;

    public Combatant(
        Guid id,
        Guid sourceId,
        CombatSide side,
        string name,
        int level,
        int maximumHealth,
        int currentHealth,
        int maximumMana,
        int currentMana,
        CombatProfile profile,
        IReadOnlyList<ProjectVictory.Domain.Combat.Rules.SkillDefinition>? skills = null,
        // PV-COMBAT-014 (aditivo, opcional).
        ProjectVictory.Domain.Combat.Rules.HealingSpellDefinition? healingSpell = null)
    {
        Skills = skills ?? Array.Empty<ProjectVictory.Domain.Combat.Rules.SkillDefinition>();
        HealingSpell = healingSpell;
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Combatant id is required.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(profile);

        if (level < 1 || maximumHealth <= 0 || maximumMana < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Invalid combatant attributes.");
        }

        Id = id;
        SourceId = sourceId;
        Side = side;
        Name = name;
        Level = level;
        MaximumHealth = maximumHealth;
        CurrentHealth = Math.Clamp(currentHealth, 0, maximumHealth);
        MaximumMana = maximumMana;
        CurrentMana = Math.Clamp(currentMana, 0, maximumMana);
        Profile = profile;
    }

    public void ApplyDamage(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        CurrentHealth = Math.Max(0, CurrentHealth - amount);
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        if (IsDead)
        {
            return;
        }

        CurrentHealth = Math.Min(MaximumHealth, CurrentHealth + amount);
    }

    public void RestoreMana(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        CurrentMana = Math.Min(MaximumMana, CurrentMana + amount);
    }

    public bool TrySpendMana(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        if (amount > CurrentMana)
        {
            return false;
        }

        CurrentMana -= amount;
        return true;
    }
}
