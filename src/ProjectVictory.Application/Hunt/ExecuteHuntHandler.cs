using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ExecuteHuntHandler
{
    private readonly IHuntRepository _huntRepository;

    private readonly ICharacterRepository _characterRepository;

    private readonly IEncounterStateStore _encounterStateStore;

    private readonly EncounterGenerator _encounterGenerator;

    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;

    private readonly PlayerOwnershipGuard _ownershipGuard;
    private readonly ProjectVictory.Application.Combat.TurnBased.ICombatSeedProvider _seedProvider;
    private readonly ProjectVictory.Application.Checkpoint.ICheckpointService _checkpoints;
    private readonly ProjectVictory.Application.GameSession.IGameActivity _activity;
    private readonly IHuntAreaCatalog _areas;
    private readonly IRuntimeRepository _runtimeRepository;
    private readonly TimeProvider _time;
    private readonly IHuntStartGate _startGate;
    private readonly ILevelTitleProvider _levelTitles;
    private readonly ProjectVictory.Application.GameSession.LoggedOutMarker _loggedOut;
    private readonly ProjectVictory.Application.GameSession.IPresenceTracker _presence;

    public ExecuteHuntHandler(
        IHuntRepository huntRepository,
        ICharacterRepository characterRepository,
        IEncounterStateStore encounterStateStore,
        EncounterGenerator encounterGenerator,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        PlayerOwnershipGuard ownershipGuard,
        ProjectVictory.Application.Combat.TurnBased.ICombatSeedProvider seedProvider,
        // PV-PERSIST-011: checkpoint de INÍCIO (Handbook §19).
        ProjectVictory.Application.Checkpoint.ICheckpointService checkpoints,
        // PV-RUNTIME-010: presença e "última Hunt escolhida" (ADR-057 v1.2).
        ProjectVictory.Application.GameSession.IGameActivity activity,
        IHuntAreaCatalog areas,
        IRuntimeRepository runtimeRepository,
        // PV-HUNT-011: horário de início (XP/hora).
        TimeProvider time,
        // PV-HUNT-013: início atômico por personagem (DEC-20261106-46).
        IHuntStartGate startGate,
        // PV-RUNTIME-013: tier do personagem para a área escolhida (R-041) e fim da marca de logout (R-054).
        ILevelTitleProvider levelTitles,
        ProjectVictory.Application.GameSession.LoggedOutMarker loggedOut,
        ProjectVictory.Application.GameSession.IPresenceTracker presence)
    {
        _presence = presence;
        _levelTitles = levelTitles;
        _loggedOut = loggedOut;
        _time = time;
        _startGate = startGate;
        _checkpoints = checkpoints;
        _activity = activity;
        _areas = areas;
        _runtimeRepository = runtimeRepository;
        _seedProvider = seedProvider;
        _ownershipGuard = ownershipGuard;

        _huntRepository = huntRepository;

        _characterRepository = characterRepository;

        _encounterStateStore = encounterStateStore;

        _encounterGenerator = encounterGenerator;

        _runtimeExecutionStateStore = runtimeExecutionStateStore;
    }

    public async Task<ExecuteHuntResult> Handle(
        ExecuteHuntCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // PV-SEC-001: o personagem precisa pertencer à conta autenticada.
        await _ownershipGuard.EnsureCharacterAsync(
            command.CharacterId,
            cancellationToken);

        var character =
            await _characterRepository.GetByIdAsync(
                command.CharacterId,
                cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        // PV-RUNTIME-013 (R-051): a área ESCOLHIDA (ou a padrão), validada no servidor antes de tudo.
        var area = ResolveArea(command.HuntArea, character);

        // PV-HUNT-013 (DEC-20261106-46, fecha o R-052): com uma Hunt em andamento, o início é RECUSADO
        // (409) e a Hunt anterior continua. A conferência e a criação ficam sob o mesmo portão do personagem.
        using var startGate =
            await _startGate.AcquireAsync(
                character.Id,
                cancellationToken);

        // Revisão do PV-EVENT-012 (ressalva 1): o logout também passa por este portão; um retorno automático
        // que chegou depois do logout é cancelado (sem Hunt e sem limpar a marca).
        if (command.AutomaticReturn && _presence.IsLoggedOut(character.Id))
        {
            throw new AutomaticReturnCancelledException();
        }

        var alreadyRunning =
            await _huntRepository.GetRunningByCharacterIdAsync(
                character.Id,
                cancellationToken);

        if (alreadyRunning is not null)
        {
            throw new HuntAlreadyRunningException(alreadyRunning.Id);
        }

        var runtimeContext =
            _runtimeExecutionStateStore.GetByCharacterId(
                character.Id);

        var hunt =
            new Domain.Hunt.Hunt(
                Guid.NewGuid(),
                character.Id,
                runtimeContext.RuntimeId);

        hunt.Start(_time.GetUtcNow());

        await _huntRepository.AddAsync(
            hunt,
            cancellationToken);

        var encounter =
            _encounterGenerator.Generate(
                hunt.Id,
                _seedProvider.NextSeed());

        _encounterStateStore.Add(
            encounter);

        // PV-RUNTIME-013 (R-054): iniciar Hunt é comando de jogo explícito — encerra a marca de logout.
        await _loggedOut.ClearAsync(character.Id, hunt.RuntimeId, cancellationToken);

        // PV-RUNTIME-010: o jogador está online e esta é a última Hunt escolhida (checkpoint mínimo).
        _activity.MarkActive(character.Id);

        // PV-RUNTIME-013 (R-051): grava a área escolhida DE VERDADE (antes: sempre a padrão).
        await _runtimeRepository.SaveLastHuntAsync(
            hunt.RuntimeId,
            area.Name,
            cancellationToken);

        // PV-PERSIST-011: checkpoint de início da Hunt (cópia do Runtime antes do primeiro combate).
        // Revisão (Server Authority): o Runtime é o da Hunt, resolvido pelo servidor a partir do
        // personagem — NUNCA o RuntimeId do corpo da requisição.
        await _checkpoints.SaveAsync(
            character.Id,
            hunt.RuntimeId,
            ProjectVictory.Application.Checkpoint.CheckpointReason.HuntStart,
            cancellationToken);

        var target =
            encounter.GetCurrentTarget();

        return new ExecuteHuntResult(
            hunt.Id,
            false,
            0,
            0,
            character.Level.Value,
            new EncounterResult(
                encounter.Id,
                encounter.HuntId,
                encounter.CurrentTargetId,
                target.Name,
                target.CurrentHealth,
                target.MaximumHealth));
    }

    // PV-RUNTIME-013 (R-051): nome do catálogo (sem diferenciar maiúsculas). A geração dos encounters
    // continua com o conteúdo padrão (única área existente) — ver contrato.
    // PV-HUNT-014 (R-064; PO-55/PO-56): o tier do título NÃO bloqueia mais a entrada — RequiredTitleTier
    // é só recomendação. Área inexistente continua recusada (400).
    private HuntAreaDefinition ResolveArea(string? requested, ProjectVictory.Domain.Entities.Character character)
    {
        if (string.IsNullOrWhiteSpace(requested))
        {
            return _areas.GetDefault();
        }

        var area = _areas.GetAll().FirstOrDefault(
            candidate => string.Equals(candidate.Name, requested.Trim(), StringComparison.OrdinalIgnoreCase));

        if (area is null)
        {
            throw new InvalidHuntAreaException(requested);
        }

        return area;
    }
}
