using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Account;

public sealed class AccountId : ValueObject
{
    public Guid Value { get; }

    public AccountId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(
                "Account identifier cannot be empty.",
                nameof(value));
        }

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static AccountId New()
    {
        return new AccountId(Guid.NewGuid());
    }
}