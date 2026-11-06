using ProjectVictory.Application.Checkpoint;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

// PV-HUNT-012 (ADR-012, ADR-057 v1.2, DEC-20261106-20): queda de MAIS de 1 min encerra a Hunt pelo
// MESMO pipeline (EndHuntHandler), com motivo próprio (Disconnect); o jogador volta à tela inicial
// "Victoria"; nada já ganho é perdido. Liga o checkpoint de logout/queda da Fila 6 (fecha o R-048).
public sealed class DisconnectHuntHandler : IDisconnectHandler
{
    private readonly IHuntRepository _hunts;
    private readonly IHuntLock _huntLock;
    private readonly EndHuntHandler _endHunt;
    private readonly ICheckpointService _checkpoints;
    private readonly IRuntimeExecutionStateStore _runtimes;

    public DisconnectHuntHandler(
        IHuntRepository hunts,
        IHuntLock huntLock,
        EndHuntHandler endHunt,
        ICheckpointService checkpoints,
        IRuntimeExecutionStateStore runtimes)
    {
        _hunts = hunts;
        _huntLock = huntLock;
        _endHunt = endHunt;
        _checkpoints = checkpoints;
        _runtimes = runtimes;
    }

    public async Task HandleAsync(PresenceSnapshot dropped, CancellationToken cancellationToken = default)
    {
        var runtimeId = _runtimes.TryGetByCharacterId(dropped.CharacterId, out var runtime) ? runtime.RuntimeId : Guid.Empty;

        // Revisão do PV-HUNT-011 (ressalva 1): encerra TODAS as Hunts em andamento do personagem (um
        // cliente que reinicia a Hunt sem sair pode ter deixado mais de uma — R-052).
        for (var guard = 0; guard < 100; guard++)
        {
            var running = await _hunts.GetRunningByCharacterIdAsync(dropped.CharacterId, cancellationToken);

            if (running is null)
            {
                break;
            }

            runtimeId = running.RuntimeId;

            // Mesmo lock do ataque e da saída manual: a queda não corre junto com uma rodada.
            using (await _huntLock.AcquireAsync(running.Id, cancellationToken))
            {
                if (await _hunts.GetByIdAsync(running.Id, cancellationToken) is { State: HuntState.Running })
                {
                    await _endHunt.Handle(new EndHuntCommand(running.Id, HuntEndReason.Disconnect), cancellationToken);
                }
                else
                {
                    _huntLock.Release(running.Id);
                }
            }
        }

        // Checkpoint de logout/queda (Handbook §19; R-048).
        await _checkpoints.SaveAsync(dropped.CharacterId, runtimeId, CheckpointReason.Logout, cancellationToken);
    }
}

public sealed record LogoutCommand;

public sealed record LogoutResult(
    Guid CharacterId,
    bool HuntEnded,
    string ReturnCity,
    EndHuntResult? Ending);

// PV-HUNT-012 (ADR-057 v1.2: "qualquer outro logout → tela inicial"): logout explícito. Encerra a
// Hunt em andamento pelo pipeline único (motivo Logout), grava o checkpoint de logout e tira a
// presença (o logout não vira "queda" nem gera retorno automático). Identidade pelo token.
public sealed class LogoutHandler
{
    private readonly ICurrentPlayerContext _currentPlayer;
    private readonly ICharacterRepository _characters;
    private readonly IHuntRepository _hunts;
    private readonly IHuntLock _huntLock;
    private readonly EndHuntHandler _endHunt;
    private readonly ICheckpointService _checkpoints;
    private readonly IPresenceTracker _presence;
    private readonly IRuntimeExecutionStateStore _runtimes;

    public LogoutHandler(
        ICurrentPlayerContext currentPlayer,
        ICharacterRepository characters,
        IHuntRepository hunts,
        IHuntLock huntLock,
        EndHuntHandler endHunt,
        ICheckpointService checkpoints,
        IPresenceTracker presence,
        IRuntimeExecutionStateStore runtimes)
    {
        _currentPlayer = currentPlayer;
        _characters = characters;
        _hunts = hunts;
        _huntLock = huntLock;
        _endHunt = endHunt;
        _checkpoints = checkpoints;
        _presence = presence;
        _runtimes = runtimes;
    }

    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character = await _characters.GetByAccountIdAsync(_currentPlayer.AccountId, cancellationToken);

        if (character is null)
        {
            return new LogoutResult(Guid.Empty, false, ProjectVictory.Application.World.HomeCity.Name, null);
        }

        EndHuntResult? ending = null;
        var running = await _hunts.GetRunningByCharacterIdAsync(character.Id, cancellationToken);
        var runtimeId = running?.RuntimeId
            ?? (_runtimes.TryGetByCharacterId(character.Id, out var runtime) ? runtime.RuntimeId : Guid.Empty);

        // Encerra todas as Hunts em andamento do personagem (R-052); a resposta traz a primeira.
        for (var guard = 0; running is not null && guard < 100; guard++)
        {
            using (await _huntLock.AcquireAsync(running.Id, cancellationToken))
            {
                if (await _hunts.GetByIdAsync(running.Id, cancellationToken) is { State: HuntState.Running })
                {
                    var result = await _endHunt.Handle(new EndHuntCommand(running.Id, HuntEndReason.Logout), cancellationToken);
                    ending ??= result;
                }
                else
                {
                    _huntLock.Release(running.Id);
                }
            }

            running = await _hunts.GetRunningByCharacterIdAsync(character.Id, cancellationToken);
        }

        await _checkpoints.SaveAsync(character.Id, runtimeId, CheckpointReason.Logout, cancellationToken);
        _presence.Remove(character.Id);

        return new LogoutResult(character.Id, ending is not null, ProjectVictory.Application.World.HomeCity.Name, ending);
    }
}
