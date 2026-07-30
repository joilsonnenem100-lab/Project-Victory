using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateRuntimeRecoveryFlowHandler
{
    private readonly IRuntimeRepository _runtimeRepository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;

    public ValidateRuntimeRecoveryFlowHandler(
        IRuntimeRepository runtimeRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore)
    {
        _runtimeRepository = runtimeRepository;
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
    }

    public async Task<ValidateRuntimeRecoveryFlowResult> Handle(
        ValidateRuntimeRecoveryFlowCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var runtime =
            await _runtimeRepository.GetByCharacterIdAsync(
                command.CharacterId,
                cancellationToken);

        if (runtime is null)
        {
            throw new InvalidOperationException(
                "Runtime not found.");
        }

        _runtimeExecutionStateStore.Store(
            new RuntimeExecutionContext(
                runtime.RuntimeId,
                runtime.CharacterId,
                runtime.Initialized,
                new CharacterRuntimeState(
                    runtime.Health,
                    runtime.Mana)));

        return new ValidateRuntimeRecoveryFlowResult(
            command.CharacterId,
            true);
    }
}
