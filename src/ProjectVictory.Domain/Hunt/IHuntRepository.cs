using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectVictory.Domain.Hunt;

public interface IHuntRepository
{
    Task AddAsync(
        Hunt hunt,
        CancellationToken cancellationToken = default);

    Task<Hunt?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    // PV-RUNTIME-010: a Hunt em andamento (Running) do personagem, se houver.
    Task<Hunt?> GetRunningByCharacterIdAsync(
        Guid characterId,
        CancellationToken cancellationToken = default);

    // PV-HUNT-011 (checklist do PV-STATE-001 D2): descarta da memória uma Hunt encerrada.
    Task RemoveAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}