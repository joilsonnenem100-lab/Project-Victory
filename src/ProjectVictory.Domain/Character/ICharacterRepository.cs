using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CharacterEntity = ProjectVictory.Domain.Entities.Character;


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


    Task<IReadOnlyList<CharacterEntity>> GetAllByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default);


    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);


    Task UpdateAsync(
        CharacterEntity character,
        CancellationToken cancellationToken = default);
}