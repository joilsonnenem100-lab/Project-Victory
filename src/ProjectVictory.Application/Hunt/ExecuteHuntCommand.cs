namespace ProjectVictory.Application.Hunt;

public sealed record ExecuteHuntCommand(
    Guid CharacterId,
    Guid RuntimeId,
    // PV-RUNTIME-013 (R-051, aditivo): área escolhida pelo jogador (nome no catálogo). Ausente = área
    // padrão (comportamento anterior). O servidor valida existência e tier (Server Authority; R-041).
    string? HuntArea = null,
    // Revisão do PV-EVENT-012 (ressalva 1): início vindo do RETORNO AUTOMÁTICO (heartbeat). Sob o portão,
    // se o personagem fez logout nesse meio-tempo, o retorno é cancelado (DEC-20261106-49). Enviado pelo
    // cliente só restringe o próprio início (nunca concede nada).
    bool AutomaticReturn = false
);