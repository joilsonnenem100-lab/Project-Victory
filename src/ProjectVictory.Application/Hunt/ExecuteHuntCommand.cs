namespace ProjectVictory.Application.Hunt;

public sealed record ExecuteHuntCommand(
    Guid CharacterId,
    Guid RuntimeId,
    // PV-RUNTIME-013 (R-051, aditivo): área escolhida pelo jogador (nome no catálogo). Ausente = área
    // padrão (comportamento anterior). O servidor valida existência e tier (Server Authority; R-041).
    string? HuntArea = null
);