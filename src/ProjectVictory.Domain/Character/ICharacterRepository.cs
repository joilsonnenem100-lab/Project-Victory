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


    // PV-RUNTIME-012 (R-056): leitura em LOTE (Recovery do startup, sem N+1). A implementação padrão
    // lê um a um (compatível); o repositório real faz uma consulta só.
    async Task<IReadOnlyList<CharacterEntity>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var found = new List<CharacterEntity>(ids.Count);

        foreach (var id in ids)
        {
            var character = await GetByIdAsync(id, cancellationToken);

            if (character is not null)
            {
                found.Add(character);
            }
        }

        return found;
    }


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