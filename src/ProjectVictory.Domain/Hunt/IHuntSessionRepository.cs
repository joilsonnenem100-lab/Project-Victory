using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectVictory.Domain.Hunt;

public interface IHuntSessionRepository
{
    Task AddAsync(
        HuntSession session,
        CancellationToken cancellationToken = default);

    Task<HuntSession?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
