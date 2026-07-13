using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.ValueObjects;

public sealed class Health : ValueObject
{
    public int Value { get; }

    public Health(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Health cannot be negative.");
        }

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}