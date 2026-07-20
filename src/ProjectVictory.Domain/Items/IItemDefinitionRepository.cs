using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectVictory.Domain.Items;

public interface IItemDefinitionRepository
{
    Task<ItemDefinition?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}