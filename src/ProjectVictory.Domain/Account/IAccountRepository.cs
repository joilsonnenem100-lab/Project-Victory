namespace ProjectVictory.Domain.Account;

public interface IAccountRepository
{
    Task AddAsync(
        Account account,
        CancellationToken cancellationToken = default);
}