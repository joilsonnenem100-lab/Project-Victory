using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class StopHuntSessionHandler
{
    private readonly IHuntSessionRepository _repository;
    private readonly PlayerOwnershipGuard _ownershipGuard;


    public StopHuntSessionHandler(
        IHuntSessionRepository repository,
        PlayerOwnershipGuard ownershipGuard)
    {
        _repository = repository;
        _ownershipGuard = ownershipGuard;
    }


    public async Task Handle(
        StopHuntSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        // PV-SEC-001: a sessão precisa ser de personagem da conta autenticada.
        await _ownershipGuard.EnsureHuntSessionAsync(
            command.HuntSessionId,
            cancellationToken);

        var session =
            await _repository.GetByIdAsync(
                command.HuntSessionId,
                cancellationToken);

        if (session is null)
        {
            throw new InvalidOperationException(
                "Hunt session not found.");
        }

        session.Stop();
    }
}
