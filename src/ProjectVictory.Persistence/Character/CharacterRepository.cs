using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Entities;

namespace ProjectVictory.Persistence.Character;

public sealed class CharacterRepository : ICharacterRepository
{
    private readonly List<Character> _characters = [];

    public Task AddAsync(
        Character character,
        CancellationToken cancellationToken = default)
    {
        _characters.Add(character);

        return Task.CompletedTask;
    }

    public Task<Character?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var character = _characters
            .FirstOrDefault(x => x.Id == id);

        return Task.FromResult(character);
    }

    public Task<Character?> GetByAccountIdAsync(
        AccountId accountId,
        CancellationToken cancellationToken = default)
    {
        var character = _characters
            .FirstOrDefault(x => x.AccountId == accountId);

        return Task.FromResult(character);
    }

    public Task UpdateAsync(
        Character character,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}