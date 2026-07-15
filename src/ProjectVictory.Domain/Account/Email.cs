using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Account;

public sealed class Email : ValueObject
{
    public string Value { get; }

    public Email(string value)
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