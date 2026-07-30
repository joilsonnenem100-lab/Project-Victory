using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.Runtime;

public sealed class RuntimeGameplaySynchronizationHandler
{
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;

    public RuntimeGameplaySynchronizationHandler(
        IRuntimeExecutionStateStore runtimeExecutionStateStore)
    {
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
    }

    public RuntimeGameplaySynchronizationResult Handle(
        RuntimeGameplaySynchronizationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!Enum.TryParse<RuntimeActivity>(
                command.Activity,
                true,
                out var activity))
        {
            throw new ArgumentException(
                "Invalid runtime activity.",
                nameof(command.Activity));
        }

        var currentContext =
            _runtimeExecutionStateStore.Get(
                command.RuntimeId);

        var updatedContext =
            currentContext with
            {
                GameplayContext =
                    new RuntimeGameplayContext(
                        activity,
                        currentContext.GameplayContext?.CurrentHuntId,
                        command.EncounterId ??
                        currentContext.GameplayContext?.CurrentEncounterId,
                        currentContext.GameplayContext?.TemporaryState)
            };

        _runtimeExecutionStateStore.Store(updatedContext);

        return new RuntimeGameplaySynchronizationResult(
            updatedContext.RuntimeId,
            updatedContext.GameplayContext.CurrentActivity.ToString(),
            updatedContext.GameplayContext.CurrentEncounterId);
    }
}
