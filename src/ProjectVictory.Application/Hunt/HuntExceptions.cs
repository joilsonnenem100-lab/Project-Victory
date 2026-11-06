namespace ProjectVictory.Application.Hunt;

// Revisão do PV-HUNT-011 (ressalva 4): Hunt já encerrada → 409 (não é erro de servidor).
public sealed class HuntAlreadyFinishedException() : InvalidOperationException("Hunt already finished.");
