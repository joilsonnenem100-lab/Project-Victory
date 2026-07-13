using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.ValueObjects;

public sealed class Experience : ValueObject
{
    public int Value { get; }

    public Experience(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Experience cannot be negative.");
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