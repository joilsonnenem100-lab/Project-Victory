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
    // SUPERADO (DEC-20261106-22): fim de suprimento NÃO encerra mais a Hunt. Mantido no enum pela
    // Regra Aditiva (valor nunca mais produzido pelo combate).
    OutOfSupplies,
    Manual,
    Disconnect,
    // PV-HUNT-012 (ADR-057 v1.2: "qualquer outro logout → tela inicial"): logout explícito (aditivo).
    Logout
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
    int CurrentMana,
    // PV-SUPPLY-011: cidade de retorno (tela inicial) e, na morte, os dados da mensagem.
    string ReturnCity = ProjectVictory.Application.World.HomeCity.Name,
    ProjectVictory.Application.World.DeathNotice? DeathNotice = null,
    // PV-HUNT-011: medição da Hunt para o XP/hora (aditivo).
    long ElapsedSeconds = 0,
    long ExperienceGained = 0,
    long ExperiencePerHour = 0);

// PV-HUNT-010: o pipeline ÚNICO de encerramento da Hunt (ADR-012: "Todo encerramento utilizará
// exatamente o mesmo pipeline, independentemente do motivo"; fluxo termina na Cidade Principal).
//
// 1. Fecha a Hunt (Victory/OutOfSupplies/Manual/Logout → Completed; Death → Failed; Disconnect → Cancelled).
//    Logout (PV-HUNT-012) é uma forma de SAÍDA MANUAL (DEC-20261106-22: morte, saída manual ou queda);
//    confirmado no PV-HUNT-013 (DEC-20261106-49): Completed, sem retorno automático.
// 2. Leva o jogador de volta à cidade e o deixa aguardando comando (atividade Idle).
// 3. Na morte: Vida e Mana cheias (knowledge-base/character.md, LOCKED: "Morte → Cidade Principal →
//    HP cheio → Mana cheia").
// 4. NADA é retirado: XP e loot já entregues pelo Reward Pipeline ficam (DEC-20261106-04;
//    character.md: "Não existe perda").
public sealed class EndHuntHandler
{
    // *** PROVISÓRIO *** — a cidade de retorno é "Victoria" (DEC-20261106-24), em dados
    // (ProjectVictory.Application.World.HomeCity). Mantido pela Regra Aditiva.
    public static RuntimeWorldContext MainCity => ProjectVictory.Application.World.HomeCity.Location;

    private readonly IHuntRepository _huntRepository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly ICharacterRepository _characterRepository;
    private readonly ProjectVictory.Application.Checkpoint.ICheckpointService _checkpoints;
    private readonly IEncounterStateStore _encounters;
    private readonly IActiveSpawnRegistry _activeSpawns;
    private readonly ProjectVictory.Application.Spawn.ISpawnSystem _spawnSystem;
    private readonly IHuntLock _huntLock;
    private readonly TimeProvider _time;
    private readonly ProjectVictory.Application.Events.IEventBus _events;

    public EndHuntHandler(
        IHuntRepository huntRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        ICharacterRepository characterRepository,
        // PV-PERSIST-011: checkpoint de CONCLUSÃO (Handbook §19).
        ProjectVictory.Application.Checkpoint.ICheckpointService checkpoints,
        // PV-HUNT-011: limpeza da memória (D2), despawn (R-044) e medição da Hunt.
        IEncounterStateStore encounters,
        IActiveSpawnRegistry activeSpawns,
        ProjectVictory.Application.Spawn.ISpawnSystem spawnSystem,
        IHuntLock huntLock,
        TimeProvider time,
        // PV-EVENT-011: o fim da Hunt é publicado como Domain Event.
        ProjectVictory.Application.Events.IEventBus events)
    {
        _events = events;
        _checkpoints = checkpoints;
        _encounters = encounters;
        _activeSpawns = activeSpawns;
        _spawnSystem = spawnSystem;
        _huntLock = huntLock;
        _time = time;
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

        // PV-PERSIST-011: checkpoint de conclusão — grava o consumo pendente e a cópia do Runtime
        // (vida/mana já restauradas na morte), uma vez, ao fim da Hunt.
        await _checkpoints.SaveAsync(
            hunt.CharacterId,
            hunt.RuntimeId,
            ProjectVictory.Application.Checkpoint.CheckpointReason.HuntConclusion,
            cancellationToken);

        // PV-HUNT-011: fim medido; encounters e Spawn Instances da Hunt descartados (D2, ADR-009/047,
        // fecha o R-044); a Hunt sai da memória (ADR-057: nunca é retomada). Nada já ganho é perdido.
        var now = _time.GetUtcNow();
        hunt.MarkEnded(now);
        var elapsed = hunt.ElapsedAt(now);

        foreach (var encounterId in _encounters.RemoveByHunt(hunt.Id))
        {
            _spawnSystem.Despawn(_activeSpawns.Release(encounterId));
        }

        await _huntRepository.RemoveAsync(hunt.Id, cancellationToken);
        _huntLock.Release(hunt.Id);

        var elapsedSeconds = (long)Math.Floor(elapsed.TotalSeconds);

        // PV-EVENT-012 (R-060): o fim da Hunt já foi gravado — assinantes do HuntEndedEvent são
        // NOTIFICADOS com falha isolada (não geram erro depois do commit).
        await _events.PublishNotificationAsync(
            new ProjectVictory.Domain.Events.HuntEndedEvent(hunt.Id, hunt.CharacterId, command.Reason.ToString(), elapsedSeconds, hunt.ExperienceGained),
            cancellationToken);
        var experiencePerHour = elapsed.TotalSeconds >= 1
            ? (long)Math.Floor(hunt.ExperienceGained * 3600d / elapsed.TotalSeconds)
            : 0;

        return new EndHuntResult(
            hunt.Id,
            command.Reason,
            hunt.State,
            true,
            state.CurrentHealth,
            state.CurrentMana,
            ProjectVictory.Application.World.HomeCity.Name,
            // PV-SUPPLY-011 (DEC-20261106-22): toda morte leva à tela inicial com a mensagem.
            command.Reason == HuntEndReason.Death ? ProjectVictory.Application.World.DeathNotice.ForDeath() : null,
            elapsedSeconds,
            hunt.ExperienceGained,
            experiencePerHour);
    }
}
