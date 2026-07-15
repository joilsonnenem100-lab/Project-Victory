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
}