using System;

namespace ProjectVictory.Domain.Hunt;

public sealed class EncounterMonsterState
{
    public Guid Id { get; }

    public Guid MonsterId { get; }

    public string Name { get; }

    public int MaximumHealth { get; }

    public int CurrentHealth { get; private set; }

    public bool IsDead =>
        CurrentHealth <= 0;


    public EncounterMonsterState(
        Guid id,
        Guid monsterId,
        string name,
        int maximumHealth)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Monster state id is required.",
                nameof(id));
        }


        if (monsterId == Guid.Empty)
        {
            throw new ArgumentException(
                "Monster id is required.",
                nameof(monsterId));
        }


        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Monster name is required.",
                nameof(name));
        }


        if (maximumHealth <= 0)
        {
            throw new ArgumentException(
                "Monster health must be greater than zero.",
                nameof(maximumHealth));
        }


        Id = id;

        MonsterId = monsterId;

        Name = name;

        MaximumHealth = maximumHealth;

        CurrentHealth = maximumHealth;
    }


    public void TakeDamage(
        int damage)
    {
        if (damage <= 0)
        {
            throw new ArgumentException(
                "Damage must be greater than zero.",
                nameof(damage));
        }


        if (IsDead)
        {
            return;
        }


        CurrentHealth -= damage;


        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }
}