using System.Collections.Concurrent;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class RuntimeExecutionStateStore :
    IRuntimeExecutionStateStore
{
    private readonly ConcurrentDictionary<Guid, RuntimeExecutionContext> _contexts = new();


    public void Store(
        RuntimeExecutionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _contexts[context.RuntimeId] = context;
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
}