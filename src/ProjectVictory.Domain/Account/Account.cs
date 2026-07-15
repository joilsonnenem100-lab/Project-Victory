using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Account;

public sealed class Account : AggregateRoot
{
    public Email Email { get; private set; }

    public PasswordHash PasswordHash { get; private set; }

    private Account(
        Guid id,
        Email email,
        PasswordHash passwordHash)
        : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
    }

    public static Account Create(
        AccountId accountId,
        Email email,
        PasswordHash passwordHash)
    {
        ArgumentNullException.ThrowIfNull(accountId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(passwordHash);

        return new Account(
            accountId.Value,
            email,
            passwordHash);
    }
}