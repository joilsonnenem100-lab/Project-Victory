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
}