using System.Collections.Concurrent;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Hunt;

public sealed class CombatVictoryResultStore : ICombatVictoryResultStore
{
    private readonly ConcurrentDictionary<Guid, CombatVictoryResult> _results =
        new();

    public void Store(
        CombatVictoryResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        _results[result.HuntId] = result;
    }

    public CombatVictoryResult Get(
        Guid huntId)
    {
        if (!_results.TryGetValue(huntId, out var result))
        {
            throw new InvalidOperationException(
                "Combat victory result not found.");
        }

        return result;
    }
}
