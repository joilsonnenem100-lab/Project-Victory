using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class CreateRuntimeExecutionHandler
{
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly IRuntimeRepository _runtimeRepository;

    public CreateRuntimeExecutionHandler(
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        IRuntimeRepository runtimeRepository)
    {
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
        _runtimeRepository = runtimeRepository;
    }

    public async Task<CreateRuntimeExecutionResult> Handle(
        CreateRuntimeExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var context =
            new RuntimeExecutionContext(
                command.RuntimeId,
                command.CharacterId,
                true,
                new CharacterRuntimeState(
                    100,
                    100),
                null,
                new RuntimeWorldContext(
                    Guid.Empty,
                    Guid.Empty,
                    Guid.Empty));

        await _runtimeRepository.AddAsync(
            command.RuntimeId,
            command.CharacterId,
            100,
            100,
            cancellationToken);

        _runtimeExecutionStateStore.Store(
            context);

        return new CreateRuntimeExecutionResult(
            command.RuntimeId,
            command.CharacterId,
            true);
    }
}
