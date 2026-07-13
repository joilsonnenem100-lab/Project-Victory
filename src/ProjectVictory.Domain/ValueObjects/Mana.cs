using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.ValueObjects;

public sealed class Mana : ValueObject
{
    public int Value { get; }

    public Mana(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Mana cannot be negative.");
        }

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}