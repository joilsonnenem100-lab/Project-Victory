using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Combat;

public sealed class ReturnCombatControlHandler
{
    private readonly ICombatContextRepository _combatContextRepository;

    public ReturnCombatControlHandler(
        ICombatContextRepository combatContextRepository)
    {
        _combatContextRepository = combatContextRepository;
    }

    public async Task<ReturnCombatControlResult> Handle(
        ReturnCombatControlCommand command,
        CancellationToken cancellationToken = default)
    {
        var combatContext = await _combatContextRepository.GetByIdAsync(
            command.CombatContextId,
            cancellationToken);

        if (combatContext is null)
        {
            throw new InvalidOperationException(
                "Combat context not found.");
        }

        combatContext.ReturnControlToHunt();

        return new ReturnCombatControlResult(
            combatContext.Id,
            combatContext.HuntId,
            true);
    }
}