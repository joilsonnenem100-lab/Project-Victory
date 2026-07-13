using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.ValueObjects;

public sealed class Level : ValueObject
{
    public int Value { get; }

    public Level(int value)
    {
        if (value < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Level cannot be less than 1.");
        }

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}