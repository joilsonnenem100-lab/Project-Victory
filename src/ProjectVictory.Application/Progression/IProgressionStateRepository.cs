using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public interface IProgressionStateRepository
{
    Task SaveAsync(
        ProgressionState state,
        CancellationToken cancellationToken = default);

    Task<ProgressionState?> GetAsync(
        Guid characterId,
        CancellationToken cancellationToken = default);
}