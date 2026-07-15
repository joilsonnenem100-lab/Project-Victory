namespace ProjectVictory.Domain.Account;

public interface IAccountRepository
{
    Task AddAsync(
        Account account,
        CancellationToken cancellationToken = default);

    Task<Account?> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default);
}