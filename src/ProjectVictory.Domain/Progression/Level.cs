using System;

namespace ProjectVictory.Domain.Progression;

public sealed class Level
{
    public int Value { get; }

    public Level(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException(
                "Level must be greater than zero.",
                nameof(value));
        }

        Value = value;
    }
}