using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Account;

public sealed class PasswordHash : ValueObject
{
    public string Value { get; }

    public PasswordHash(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}