using ProjectVictory.Domain.Account;

namespace ProjectVictory.Persistence.Account;

public sealed class AccountRepository : IAccountRepository
{
    private readonly List<Domain.Account.Account> _accounts = [];

    public Task AddAsync(
        Domain.Account.Account account,
        CancellationToken cancellationToken = default)
    {
        _accounts.Add(account);

        return Task.CompletedTask;
    }

    public Task<Domain.Account.Account?> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        var account = _accounts.FirstOrDefault(
            x => x.Email.Equals(email));

        return Task.FromResult(account);
    }
}