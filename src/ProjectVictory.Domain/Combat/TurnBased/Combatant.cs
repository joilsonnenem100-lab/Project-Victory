using System;

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
    int Defense);

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
        CombatProfile profile)
    {
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
