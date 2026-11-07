using System.Collections.Concurrent;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.GameSession;

// PV-RUNTIME-010 (ADR-057 v1.2; DEC-20261106-20): tempos da sessão de jogo, em CONFIGURAÇÃO.
// - DisconnectTimeout: sem heartbeat por MAIS que isso = queda (padrão 1 min); no primeiro minuto
//   a Hunt segue.
// - ReturnDelay: depois de uma queda, MAIS que isso online = volta à última Hunt escolhida com uma
//   NOVA Hunt (padrão 10 s).
// - SweepInterval: de quanto em quanto tempo o servidor procura quedas (0 = sem varredura automática).
public sealed record GameSessionSettings(
    TimeSpan DisconnectTimeout,
    TimeSpan ReturnDelay,
    TimeSpan SweepInterval)
{
    public const string DisconnectTimeoutKey = "GameSession:DisconnectTimeoutSeconds";
    public const string ReturnDelayKey = "GameSession:ReturnDelaySeconds";
    public const string SweepIntervalKey = "GameSession:SweepIntervalSeconds";

    public static GameSessionSettings Default { get; } =
        new(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(5));
}

// PV-RUNTIME-010: presença de um personagem (estado Temporário, em memória).
public sealed record PresenceSnapshot(
    Guid AccountId,
    Guid CharacterId,
    DateTimeOffset LastSeen,
    DateTimeOffset OnlineSince,
    bool Dropped,
    bool PendingReturn);

// PV-RUNTIME-010 (diretriz econômica do M6): presença em memória THREAD-SAFE, sem WebSocket. Cada
// heartbeat (ou comando de jogo) renova a presença; a varredura marca como queda quem passou do limite.
public interface IPresenceTracker
{
    // Registra que o personagem está online agora. Se ele estava "caído", começa a contagem de retorno.
    void Touch(Guid accountId, Guid characterId, DateTimeOffset now);

    // Marca e devolve os personagens sem sinal há MAIS que `timeout` (cada queda é devolvida uma vez).
    IReadOnlyList<PresenceSnapshot> CollectDropped(DateTimeOffset now, TimeSpan timeout);

    // Depois de uma queda: online há MAIS que `delay`?
    bool IsReturnDue(Guid characterId, DateTimeOffset now, TimeSpan delay);

    void ClearPendingReturn(Guid characterId);

    // Revisão do PV-RUNTIME-010 (ressalva 1): consome o retorno de forma ATÔMICA — só uma chamada
    // concorrente recebe true (evita duas Hunts com dois heartbeats ao mesmo tempo).
    bool TryConsumeReturn(Guid characterId, DateTimeOffset now, TimeSpan delay);

    void Remove(Guid characterId);

    PresenceSnapshot? Get(Guid characterId);

    // Revisão do PV-HUNT-012 (ressalva 2): desfaz a marcação de queda (o tratamento falhou), para a
    // próxima varredura detectar de novo. Não mexe no LastSeen.
    void UnmarkDropped(Guid characterId);

    // PV-RUNTIME-012 (DEC-20261106-47): reinício do servidor = queda. O Recovery marca o personagem
    // como caído; o próximo Touch (heartbeat) começa a contagem do retorno, como numa queda comum.
    void MarkDroppedByRestart(Guid accountId, Guid characterId, DateTimeOffset now);

    // PV-RUNTIME-013 (R-054): marca de logout (memória) — heartbeat ignorado até o próximo comando de jogo.
    void MarkLoggedOut(Guid characterId);

    // Devolve true se havia marca.
    bool ClearLoggedOut(Guid characterId);

    bool IsLoggedOut(Guid characterId);
}

public sealed class PresenceTracker : IPresenceTracker
{
    private readonly ConcurrentDictionary<Guid, PresenceSnapshot> _presence = new();

    // Revisão do PV-RUNTIME-013 (ressalva 1): personagem deslogado não ganha presença (um heartbeat que
    // passou pela conferência antes do logout não recria a presença depois dele).
    public void Touch(Guid accountId, Guid characterId, DateTimeOffset now)
    {
        if (_loggedOut.ContainsKey(characterId))
        {
            return;
        }

        TouchCore(accountId, characterId, now);
    }

    private void TouchCore(Guid accountId, Guid characterId, DateTimeOffset now) =>
        _presence.AddOrUpdate(
            characterId,
            _ => new PresenceSnapshot(accountId, characterId, now, now, false, false),
            (_, current) => current.Dropped
                ? current with { LastSeen = now, OnlineSince = now, Dropped = false, PendingReturn = true }
                : current with { LastSeen = now });

    public IReadOnlyList<PresenceSnapshot> CollectDropped(DateTimeOffset now, TimeSpan timeout)
    {
        var dropped = new List<PresenceSnapshot>();

        foreach (var (characterId, current) in _presence)
        {
            if (current.Dropped || now - current.LastSeen <= timeout)
            {
                continue;
            }

            // Revisão do PV-RUNTIME-013 (ressalva 1): presença que sobrou de um deslogado não vira queda.
            if (_loggedOut.ContainsKey(characterId))
            {
                ((ICollection<KeyValuePair<Guid, PresenceSnapshot>>)_presence).Remove(new KeyValuePair<Guid, PresenceSnapshot>(characterId, current));
                continue;
            }

            var marked = current with { Dropped = true, PendingReturn = false };

            // Só quem conseguir trocar o valor "vence" (um heartbeat concorrente cancela a queda).
            if (_presence.TryUpdate(characterId, marked, current))
            {
                dropped.Add(marked);
            }
        }

        return dropped;
    }

    public bool IsReturnDue(Guid characterId, DateTimeOffset now, TimeSpan delay) =>
        _presence.TryGetValue(characterId, out var current)
        && current is { Dropped: false, PendingReturn: true }
        && now - current.OnlineSince > delay;

    public void ClearPendingReturn(Guid characterId)
    {
        while (_presence.TryGetValue(characterId, out var current) && current.PendingReturn)
        {
            if (_presence.TryUpdate(characterId, current with { PendingReturn = false }, current))
            {
                return;
            }
        }
    }

    public bool TryConsumeReturn(Guid characterId, DateTimeOffset now, TimeSpan delay)
    {
        while (_presence.TryGetValue(characterId, out var current)
            && current is { Dropped: false, PendingReturn: true }
            && now - current.OnlineSince > delay)
        {
            if (_presence.TryUpdate(characterId, current with { PendingReturn = false }, current))
            {
                return true;
            }
        }

        return false;
    }

    public void Remove(Guid characterId) => _presence.TryRemove(characterId, out _);

    public void UnmarkDropped(Guid characterId)
    {
        while (_presence.TryGetValue(characterId, out var current) && current.Dropped)
        {
            if (_presence.TryUpdate(characterId, current with { Dropped = false }, current))
            {
                return;
            }
        }
    }

    private readonly ConcurrentDictionary<Guid, byte> _loggedOut = new();

    public void MarkLoggedOut(Guid characterId) => _loggedOut[characterId] = 0;

    public bool ClearLoggedOut(Guid characterId) => _loggedOut.TryRemove(characterId, out _);

    public bool IsLoggedOut(Guid characterId) => _loggedOut.ContainsKey(characterId);

    public void MarkDroppedByRestart(Guid accountId, Guid characterId, DateTimeOffset now) =>
        _presence[characterId] = new PresenceSnapshot(accountId, characterId, now, now, true, false);

    public PresenceSnapshot? Get(Guid characterId) =>
        _presence.TryGetValue(characterId, out var current) ? current : null;
}

// PV-RUNTIME-010: Runtime Coordinator — coordena a experiência contínua enquanto o jogador está
// conectado e detecta a queda, sem inflar os handlers. O encerramento da Hunt na queda é ligado no
// PV-HUNT-012 (IDisconnectHandler).
public interface IDisconnectHandler
{
    Task HandleAsync(PresenceSnapshot dropped, CancellationToken cancellationToken = default);
}

// PV-EVENT-012 (fecha o R-060): um ESCOPO de DI (e portanto um DbContext) por queda — a falha no
// tratamento de uma queda não deixa alterações parciais rastreadas para o SaveChanges da seguinte.
public interface IDisconnectScope : IAsyncDisposable
{
    IEnumerable<IDisconnectHandler> Handlers { get; }
}

public interface IDisconnectScopeFactory
{
    IDisconnectScope Create();
}

public sealed class GameSessionCoordinator
{
    private readonly IPresenceTracker _presence;
    private readonly GameSessionSettings _settings;
    private readonly TimeProvider _time;
    private readonly IEnumerable<IDisconnectHandler> _disconnectHandlers;
    private readonly IDisconnectScopeFactory? _scopes;

    // Sem fábrica de escopos (testes de unidade): os handlers recebidos atendem todas as quedas.
    public GameSessionCoordinator(
        IPresenceTracker presence,
        GameSessionSettings settings,
        TimeProvider time,
        IEnumerable<IDisconnectHandler> disconnectHandlers)
        : this(presence, settings, time, disconnectHandlers, null)
    {
    }

    // PV-EVENT-012 (R-060): composição oficial — um escopo novo por queda.
    public GameSessionCoordinator(
        IPresenceTracker presence,
        GameSessionSettings settings,
        TimeProvider time,
        IEnumerable<IDisconnectHandler> disconnectHandlers,
        IDisconnectScopeFactory? scopes)
    {
        _presence = presence;
        _settings = settings;
        _time = time;
        _disconnectHandlers = disconnectHandlers;
        _scopes = scopes;
    }

    // Quedas cujo tratamento falhou (observabilidade e testes).
    public int Failures { get; private set; }

    // Revisão do PV-EVENT-010/011 (ressalva 1): os erros da última varredura, para o host registrar no log.
    public IReadOnlyList<(PresenceSnapshot Presence, Exception Error)> Errors => _errors;

    private readonly List<(PresenceSnapshot Presence, Exception Error)> _errors = [];

    // Uma varredura: detecta as quedas (sem sinal por MAIS que o limite) e as entrega aos handlers.
    public async Task<IReadOnlyList<PresenceSnapshot>> SweepAsync(CancellationToken cancellationToken = default)
    {
        _errors.Clear(); // só os erros DESTA varredura (revisão do PV-GOV-011, ressalva 7)
        var dropped = _presence.CollectDropped(_time.GetUtcNow(), _settings.DisconnectTimeout);

        var handled = new List<PresenceSnapshot>();

        // Revisão do PV-HUNT-012 (ressalva 2): uma queda com erro não impede as outras e volta a ser
        // detectada na próxima varredura (a Hunt não fica presa em Running).
        foreach (var presence in dropped)
        {
            try
            {
                if (_scopes is null)
                {
                    foreach (var handler in _disconnectHandlers)
                    {
                        await handler.HandleAsync(presence, cancellationToken);
                    }
                }
                else
                {
                    await using var scope = _scopes.Create();

                    foreach (var handler in scope.Handlers)
                    {
                        await handler.HandleAsync(presence, cancellationToken);
                    }
                }

                handled.Add(presence);
            }
            catch (Exception error) when (!cancellationToken.IsCancellationRequested)
            {
                _presence.UnmarkDropped(presence.CharacterId);
                Failures++;
                _errors.Add((presence, error));
            }
        }

        return handled;
    }
}

public sealed record HeartbeatCommand;

// PV-RUNTIME-010: resposta do heartbeat (o cliente só exibe; o servidor decide).
public sealed record HeartbeatResult(
    Guid CharacterId,
    string Status,
    Guid? HuntId,
    bool ReturnedToLastHunt,
    string? LastHuntArea,
    EncounterResult? Encounter);

// PV-RUNTIME-010: heartbeat HTTP (sem WebSocket). Identidade pelo TOKEN (ICurrentPlayerContext).
// Depois de uma queda, com MAIS que ReturnDelay online e sem Hunt em andamento, o servidor inicia uma
// NOVA Hunt na última Hunt escolhida (ADR-057 v1.2) — a Hunt anterior nunca é retomada.
public sealed class HeartbeatHandler
{
    private readonly ICurrentPlayerContext _currentPlayer;
    private readonly ICharacterRepository _characters;
    private readonly IPresenceTracker _presence;
    private readonly IHuntRepository _hunts;
    private readonly IRuntimeRepository _runtimes;
    private readonly IRuntimeExecutionStateStore _runtimeStates;
    private readonly ExecuteHuntHandler _executeHunt;
    private readonly GameSessionSettings _settings;
    private readonly TimeProvider _time;

    public HeartbeatHandler(
        ICurrentPlayerContext currentPlayer,
        ICharacterRepository characters,
        IPresenceTracker presence,
        IHuntRepository hunts,
        IRuntimeRepository runtimes,
        IRuntimeExecutionStateStore runtimeStates,
        ExecuteHuntHandler executeHunt,
        GameSessionSettings settings,
        TimeProvider time)
    {
        _currentPlayer = currentPlayer;
        _characters = characters;
        _presence = presence;
        _hunts = hunts;
        _runtimes = runtimes;
        _runtimeStates = runtimeStates;
        _executeHunt = executeHunt;
        _settings = settings;
        _time = time;
    }

    public async Task<HeartbeatResult> Handle(HeartbeatCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var accountId = _currentPlayer.AccountId;
        var character = await _characters.GetByAccountIdAsync(accountId, cancellationToken);

        // Revisão do PV-RUNTIME-010 (ressalva 3): conta sem personagem não é erro de servidor.
        if (character is null)
        {
            return new HeartbeatResult(Guid.Empty, "NoCharacter", null, false, null, null);
        }

        // PV-RUNTIME-013 (fecha o R-054): depois do logout o heartbeat NÃO recria a presença (fechar a
        // página não vira "queda"); só "entrar no jogo" ou iniciar Hunt encerram a marca.
        if (_presence.IsLoggedOut(character.Id))
        {
            return new HeartbeatResult(character.Id, "LoggedOut", null, false, null, null);
        }

        var now = _time.GetUtcNow();
        _presence.Touch(accountId, character.Id, now);

        var running = await _hunts.GetRunningByCharacterIdAsync(character.Id, cancellationToken);
        var runtime = await _runtimes.GetByCharacterIdAsync(character.Id, cancellationToken);

        // Revisão do PV-RUNTIME-010 (ressalva 2): se já há Hunt em andamento quando o retorno vence, o
        // retorno é consumido (não inicia outra Hunt depois que esta acabar).
        if (running is not null)
        {
            _presence.TryConsumeReturn(character.Id, now, _settings.ReturnDelay);
        }
        else if (runtime?.LastHuntArea is not null
            && _runtimeStates.Contains(runtime.RuntimeId)
            && _presence.TryConsumeReturn(character.Id, now, _settings.ReturnDelay))
        {
            try
            {
                // PV-RUNTIME-013 (R-051): volta à área da última Hunt escolhida, não à padrão.
                var started = await _executeHunt.Handle(
                    new ExecuteHuntCommand(character.Id, runtime.RuntimeId, runtime.LastHuntArea, AutomaticReturn: true),
                    cancellationToken);

                return new HeartbeatResult(character.Id, "Online", started.HuntId, true, runtime.LastHuntArea, started.Encounter);
            }
            catch (HuntAlreadyRunningException conflict)
            {
                // PV-HUNT-013: o jogador iniciou uma Hunt entre a conferência e o retorno — o retorno é
                // consumido (como na ressalva 2 do PV-RUNTIME-010) e a Hunt dele continua.
                return new HeartbeatResult(character.Id, "Online", conflict.RunningHuntId, false, runtime.LastHuntArea, null);
            }
            catch (AutomaticReturnCancelledException)
            {
                // Revisão do PV-EVENT-012 (ressalva 1): logout entre a conferência e o início.
                return new HeartbeatResult(character.Id, "LoggedOut", null, false, null, null);
            }
            catch (InvalidHuntAreaException)
            {
                // PV-RUNTIME-013: a área guardada deixou de existir/estar disponível — o retorno é
                // consumido sem Hunt (o jogador escolhe outra na tela inicial).
                return new HeartbeatResult(character.Id, "Online", null, false, runtime.LastHuntArea, null);
            }
        }

        return new HeartbeatResult(character.Id, "Online", running?.Id, false, runtime?.LastHuntArea, null);
    }
}
