using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.Runtime;

public sealed class UpdateRuntimeActivityHandler
{
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;

    public UpdateRuntimeActivityHandler(
        IRuntimeExecutionStateStore runtimeExecutionStateStore)
    {
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
    }

    public UpdateRuntimeActivityResult Handle(
        UpdateRuntimeActivityCommand command)
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

        var currentActivity =
            currentContext.GameplayContext?.CurrentActivity
            ?? RuntimeActivity.Idle;

        if (!RuntimeActivityTransitionRules.CanTransition(
                currentActivity,
                activity))
        {
            throw new InvalidOperationException(
                $"Invalid runtime activity transition: {currentActivity} -> {activity}.");
        }

        var updatedContext =
            currentContext with
            {
                GameplayContext =
                    new RuntimeGameplayContext(
                        activity,
                        currentContext.GameplayContext?.CurrentHuntId,
                        currentContext.GameplayContext?.CurrentEncounterId,
                        currentContext.GameplayContext?.TemporaryState)
            };

        _runtimeExecutionStateStore.Store(
            updatedContext);

        return new UpdateRuntimeActivityResult(
            updatedContext.RuntimeId,
            updatedContext.GameplayContext.CurrentActivity.ToString());
    }
}
