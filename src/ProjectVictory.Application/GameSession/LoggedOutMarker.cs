using ProjectVictory.Application.VerticalSlice;

namespace ProjectVictory.Application.GameSession;

// PV-RUNTIME-013 (fecha o R-054; DEC-20261106-49): o logout marca o personagem como DESLOGADO até o
// próximo comando de jogo explícito ("entrar no jogo" ou iniciar Hunt). Enquanto deslogado, o heartbeat
// é ignorado (não recria a presença): fechar a página depois do logout NÃO vira "queda" e não gera
// retorno automático. A marca fica na memória (presença) e no checkpoint do Runtime (coluna LoggedOut),
// para sobreviver ao reinício do servidor (o Recovery não agenda retorno para quem saiu — DEC-20261106-47).
public sealed class LoggedOutMarker
{
    private readonly IPresenceTracker _presence;
    private readonly IRuntimeRepository _runtimes;

    public LoggedOutMarker(IPresenceTracker presence, IRuntimeRepository runtimes)
    {
        _presence = presence;
        _runtimes = runtimes;
    }

    public async Task MarkAsync(Guid characterId, Guid runtimeId, CancellationToken cancellationToken = default)
    {
        // Revisão do PV-RUNTIME-013 (ressalva 1): marca ANTES de remover — um Touch concorrente é ignorado
        // pelo próprio tracker enquanto houver marca.
        _presence.MarkLoggedOut(characterId);
        _presence.Remove(characterId);

        if (runtimeId != Guid.Empty)
        {
            await _runtimes.SaveLoggedOutAsync(runtimeId, true, cancellationToken);
        }
    }

    // Só grava quando havia marca (sem escrita extra em cada "entrar"/início de Hunt).
    // Revisão do PV-RUNTIME-013 (ressalva 3): `persisted` = a marca lida do checkpoint; grava `false`
    // também quando só o banco a tinha (memória e banco não divergem).
    public async Task ClearAsync(Guid characterId, Guid runtimeId, CancellationToken cancellationToken = default, bool persisted = false)
    {
        var hadMark = _presence.ClearLoggedOut(characterId);

        if ((hadMark || persisted) && runtimeId != Guid.Empty)
        {
            await _runtimes.SaveLoggedOutAsync(runtimeId, false, cancellationToken);
        }
    }
}
