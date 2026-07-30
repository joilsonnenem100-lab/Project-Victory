using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectVictory.Domain.Items;

public interface IItemInstanceRepository
{
    Task<ItemInstance?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}