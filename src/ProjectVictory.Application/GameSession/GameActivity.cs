using ProjectVictory.Application.Session;

namespace ProjectVictory.Application.GameSession;

// PV-RUNTIME-010: todo comando de jogo autenticado conta como sinal de presença (além do heartbeat),
// para uma Hunt iniciada sem heartbeat também ser acompanhada. Identidade pelo token.
public interface IGameActivity
{
    void MarkActive(Guid characterId);
}

public sealed class GameActivity : IGameActivity
{
    private readonly ICurrentPlayerContext _currentPlayer;
    private readonly IPresenceTracker _presence;
    private readonly TimeProvider _time;

    public GameActivity(ICurrentPlayerContext currentPlayer, IPresenceTracker presence, TimeProvider time)
    {
        _currentPlayer = currentPlayer;
        _presence = presence;
        _time = time;
    }

    public void MarkActive(Guid characterId) =>
        _presence.Touch(_currentPlayer.AccountId, characterId, _time.GetUtcNow());
}
