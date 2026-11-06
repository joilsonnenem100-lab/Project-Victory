using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed record LeaveHuntCommand(Guid HuntId);

// PV-HUNT-011 (DEC-20261106-22): saída MANUAL — um dos três jeitos de terminar a Hunt (morte, saída,
// queda), pelo pipeline único (ADR-012). Server Authority: a Hunt precisa ser de personagem da conta.
public sealed class LeaveHuntHandler
{
    private readonly PlayerOwnershipGuard _ownershipGuard;
    private readonly IHuntRepository _hunts;
    private readonly IHuntLock _huntLock;
    private readonly EndHuntHandler _endHunt;

    public LeaveHuntHandler(
        PlayerOwnershipGuard ownershipGuard,
        IHuntRepository hunts,
        IHuntLock huntLock,
        EndHuntHandler endHunt)
    {
        _ownershipGuard = ownershipGuard;
        _hunts = hunts;
        _huntLock = huntLock;
        _endHunt = endHunt;
    }

    public async Task<EndHuntResult> Handle(LeaveHuntCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Revisão do PV-HUNT-011 (ressalva 4): Hunt inexistente responde como a de outra conta (403),
        // sem revelar se o id existe.
        var hunt =
            await _hunts.GetByIdAsync(command.HuntId, cancellationToken)
            ?? throw new ProjectVictory.Application.Session.OwnershipViolationException("hunt");

        await _ownershipGuard.EnsureCharacterAsync(hunt.CharacterId, cancellationToken);

        using (await _huntLock.AcquireAsync(hunt.Id, cancellationToken))
        {
            if (await _hunts.GetByIdAsync(hunt.Id, cancellationToken) is not { State: HuntState.Running })
            {
                _huntLock.Release(hunt.Id); // ressalva 6: não deixa lock de Hunt morta
                throw new HuntAlreadyFinishedException();
            }

            return await _endHunt.Handle(new EndHuntCommand(hunt.Id, HuntEndReason.Manual), cancellationToken);
        }
    }
}
