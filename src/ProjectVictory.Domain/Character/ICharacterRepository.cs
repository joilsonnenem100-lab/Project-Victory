using System;
using System.Threading;
using System.Threading.Tasks;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Domain.Character;

public interface ICharacterRepository
{
    Task AddAsync(
        CharacterEntity character,
        CancellationToken cancellationToken = default);

    Task<CharacterEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<CharacterEntity?> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        CharacterEntity character,
        CancellationToken cancellationToken = default);
}