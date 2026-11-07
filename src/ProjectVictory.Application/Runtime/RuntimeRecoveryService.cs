using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.Runtime;

public sealed record RuntimeRecoveryReport(
    int Recovered,
    int SkippedWithoutCharacter);

// PV-RUNTIME-011 (Handbook §18; ADR-057, Parte A simplificada): ao iniciar o servidor, carrega o
// checkpoint do PERSONAGEM e do RUNTIME e reconstrói o contexto em memória. NÃO recupera Hunt Session
// (a Hunt nunca é retomada): todo Runtime volta à tela inicial "Victoria", aguardando comando (Idle),
// com vida/mana do checkpoint limitadas ao máximo do personagem.
public sealed class RuntimeRecoveryService
{
    private readonly IRuntimeRepository _runtimes;
    private readonly ICharacterRepository _characters;
    private readonly IRuntimeExecutionStateStore _store;

    public RuntimeRecoveryService(
        IRuntimeRepository runtimes,
        ICharacterRepository characters,
        IRuntimeExecutionStateStore store)
    {
        _runtimes = runtimes;
        _characters = characters;
        _store = store;
    }

    public async Task<RuntimeRecoveryReport> RecoverAsync(CancellationToken cancellationToken = default)
    {
        var recovered = 0;
        var skipped = 0;

        foreach (var runtime in await _runtimes.GetAllAsync(cancellationToken))
        {
            var character = await _characters.GetByIdAsync(runtime.CharacterId, cancellationToken);

            // Runtime órfão (personagem inexistente) não é reconstruído.
            if (character is null)
            {
                skipped++;
                continue;
            }

            _store.Store(new RuntimeExecutionContext(
                runtime.RuntimeId,
                runtime.CharacterId,
                runtime.Initialized,
                new CharacterRuntimeState(
                    Math.Clamp(runtime.Health, 0, character.MaximumHealth.Value),
                    Math.Clamp(runtime.Mana, 0, character.MaximumMana.Value)),
                new RuntimeGameplayContext(RuntimeActivity.Idle),
                ProjectVictory.Application.World.HomeCity.Location));

            recovered++;
        }

        return new RuntimeRecoveryReport(recovered, skipped);
    }
}
