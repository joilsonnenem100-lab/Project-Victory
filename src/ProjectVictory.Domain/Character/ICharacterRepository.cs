using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectVictory.Domain.Entities;

namespace ProjectVictory.Domain.Character;

public interface ICharacterRepository
{
    Task AddAsync(
        Entities.Character character,
        CancellationToken cancellationToken = default);

    Task<Entities.Character?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Entities.Character?> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default);
}