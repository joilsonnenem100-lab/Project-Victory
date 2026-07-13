using System;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.ValueObjects;

namespace ProjectVictory.Domain.Entities;

public sealed class Character : AggregateRoot
{
    public Health Health { get; }

    public Mana Mana { get; }

    public Character(Guid id, Health health, Mana mana)
        : base(id)
    {
        Health = health;
        Mana = mana;
    }
}