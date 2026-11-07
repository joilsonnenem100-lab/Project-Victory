namespace ProjectVictory.Application.Hunt;

// Revisão do PV-HUNT-011 (ressalva 4): Hunt já encerrada → 409 (não é erro de servidor).
public sealed class HuntAlreadyFinishedException() : InvalidOperationException("Hunt already finished.");


// PV-RUNTIME-013 (R-051/R-041): área desconhecida ou acima do tier do personagem → 400.
public sealed class InvalidHuntAreaException(string? area)
    : InvalidOperationException("Hunt area not available.")
{
    public string? Area { get; } = area;
}

// PV-HUNT-013 (DEC-20261106-46, fecha o R-052): iniciar uma Hunt com outra em andamento → 409, e a
// anterior NÃO é encerrada.
public sealed class HuntAlreadyRunningException(Guid runningHuntId)
    : InvalidOperationException("Hunt already running.")
{
    public Guid RunningHuntId { get; } = runningHuntId;
}
