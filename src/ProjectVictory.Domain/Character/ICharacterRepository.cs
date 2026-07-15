using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Entities;

namespace ProjectVictory.Domain.Character;

public interface ICharacterRepository
{
    Task AddAsync(
        Character character,
        CancellationToken cancellationToken = default);

    Task<Character?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Character?> GetByAccountIdAsync(
        AccountId accountId,
        CancellationToken cancellationToken = default);
}