using System.Collections.Concurrent;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class RuntimeExecutionStateStore :
    IRuntimeExecutionStateStore
{
    private readonly ConcurrentDictionary<Guid, RuntimeExecutionContext> _contexts = new();

    private readonly ConcurrentDictionary<Guid, Guid> _characterRuntimeIndex = new();

    public void Store(
        RuntimeExecutionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _contexts[context.RuntimeId] = context;

        _characterRuntimeIndex[context.CharacterId] =
            context.RuntimeId;
    }

    public RuntimeExecutionContext Get(
        Guid runtimeId)
    {
        if (!_contexts.TryGetValue(
                runtimeId,
                out var context))
        {
            throw new InvalidOperationException(
                "Runtime execution context not found.");
        }

        return context;
    }

    public bool Contains(
        Guid runtimeId)
    {
        return _contexts.ContainsKey(runtimeId);
    }

    public RuntimeExecutionContext GetByCharacterId(
        Guid characterId)
    {
        if (!_characterRuntimeIndex.TryGetValue(
                characterId,
                out var runtimeId))
        {
            throw new InvalidOperationException(
                "Runtime execution context not found for character.");
        }

        return Get(runtimeId);
    }
}