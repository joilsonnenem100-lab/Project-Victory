using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.ValueObjects;

public sealed class Experience : ValueObject
{
    // PV-PROG-010: long (DEC-20261106-10) — a Experience absoluta passa do limite de int
    // depois do Level 500 (ADR-023: nunca reinicia).
    public long Value { get; }

    public Experience(long value)
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