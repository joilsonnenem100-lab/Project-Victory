using System.Collections.Concurrent;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Hunt;

public sealed class CombatResultStore : ICombatResultStore
{
    private readonly ConcurrentDictionary<Guid, CombatResult> _results =
        new();


    public void Store(
        CombatResult result)
    {
        ArgumentNullException.ThrowIfNull(result);


        _results[result.HuntId] =
            result;
    }


    public CombatResult Get(
        Guid huntId)
    {
        if (!_results.TryGetValue(
                huntId,
                out var result))
        {
            throw new InvalidOperationException(
                "Combat result not found.");
        }


        return result;
    }
}