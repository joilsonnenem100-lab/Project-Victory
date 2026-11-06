using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.Hunt;

// PV-HUNT-010: motivos de encerramento. Todos passam pelo MESMO pipeline (ADR-012).
public enum HuntEndReason
{
    Victory,
    Death,
    OutOfSupplies,
    Manual,
    Disconnect
}

public sealed record EndHuntCommand(
    Guid HuntId,
    HuntEndReason Reason);

public sealed record EndHuntResult(
    Guid HuntId,
    HuntEndReason Reason,
    HuntState HuntState,
    bool ReturnedToCity,
    int CurrentHealth,
    int CurrentMana);

// PV-HUNT-010: o pipeline ÚNICO de encerramento da Hunt (ADR-012: "Todo encerramento utilizará
// exatamente o mesmo pipeline, independentemente do motivo"; fluxo termina na Cidade Principal).
//
// 1. Fecha a Hunt (Victory/OutOfSupplies/Manual → Completed; Death → Failed; Disconnect → Cancelled).
// 2. Leva o jogador de volta à cidade e o deixa aguardando comando (atividade Idle).
// 3. Na morte: Vida e Mana cheias (knowledge-base/character.md, LOCKED: "Morte → Cidade Principal →
//    HP cheio → Mana cheia").
// 4. NADA é retirado: XP e loot já entregues pelo Reward Pipeline ficam (DEC-20261106-04;
//    character.md: "Não existe perda").
public sealed class EndHuntHandler
{
    // *** PROVISÓRIO *** — não existe "Cidade Principal" no mundo semeado (cidades são Fase 7). A
    // única localização ativa, "Starting Location" (WorldDefinitionSeed), faz o papel de cidade.
    public static RuntimeWorldContext MainCity { get; } =
        new(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            Guid.Parse("30000000-0000-0000-0000-000000000001"));

    private readonly IHuntRepository _huntRepository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly ICharacterRepository _characterRepository;

    public EndHuntHandler(
        IHuntRepository huntRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        ICharacterRepository characterRepository)
    {
        _huntRepository = huntRepository;
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
        _characterRepository = characterRepository;
    }

    public async Task<EndHuntResult> Handle(
        EndHuntCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var hunt =
            await _huntRepository.GetByIdAsync(command.HuntId, cancellationToken)
            ?? throw new InvalidOperationException("Hunt not found.");

        if (hunt.State == HuntState.Running)
        {
            switch (command.Reason)
            {
                case HuntEndReason.Death:
                    hunt.Fail();
                    break;
                case HuntEndReason.Disconnect:
                    hunt.Cancel();
                    break;
                default:
                    hunt.Complete();
                    break;
            }
        }

        var runtime = _runtimeExecutionStateStore.Get(hunt.RuntimeId);
        var state = runtime.CharacterState ?? new CharacterRuntimeState(0, 0);

        if (command.Reason == HuntEndReason.Death)
        {
            var character =
                await _characterRepository.GetByIdAsync(hunt.CharacterId, cancellationToken)
                ?? throw new InvalidOperationException("Character not found.");

            state.UpdateHealth(character.MaximumHealth.Value);
            state.UpdateMana(character.MaximumMana.Value);
        }

        _runtimeExecutionStateStore.Store(
            runtime with
            {
                CharacterState = state,
                GameplayContext = new RuntimeGameplayContext(RuntimeActivity.Idle),
                WorldContext = MainCity
            });

        return new EndHuntResult(
            hunt.Id,
            command.Reason,
            hunt.State,
            true,
            state.CurrentHealth,
            state.CurrentMana);
    }
}
