using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class CreateRuntimeExecutionHandler
{
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly IRuntimeRepository _runtimeRepository;
    private readonly PlayerOwnershipGuard _ownershipGuard;

    public CreateRuntimeExecutionHandler(
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        IRuntimeRepository runtimeRepository,
        PlayerOwnershipGuard ownershipGuard)
    {
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
        _runtimeRepository = runtimeRepository;
        _ownershipGuard = ownershipGuard;
    }

    public async Task<CreateRuntimeExecutionResult> Handle(
        CreateRuntimeExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // PV-SEC-001: o personagem precisa pertencer à conta autenticada.
        await _ownershipGuard.EnsureCharacterAsync(
            command.CharacterId,
            cancellationToken);

        // PV-STATE-001: o RuntimeId vem do cliente; um id já em uso nunca é sobrescrito.
        // Antes a proteção dependia de a gravação no banco falhar (chave primária) antes do Store.
        // Runtime de outro personagem → 403 (mesma regra do PV-SEC-001); do mesmo → erro.
        if (_runtimeExecutionStateStore.Contains(command.RuntimeId))
        {
            var existing =
                _runtimeExecutionStateStore.Get(
                    command.RuntimeId);

            if (existing.CharacterId != command.CharacterId)
            {
                throw new OwnershipViolationException("runtime");
            }

            throw new InvalidOperationException(
                "Runtime already exists.");
        }

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
