using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Combat;

public sealed class ExecuteCombatActionHandler
{
    private readonly ICombatContextRepository _combatContextRepository;

    public ExecuteCombatActionHandler(
        ICombatContextRepository combatContextRepository)
    {
        _combatContextRepository = combatContextRepository;
    }

    public async Task<CombatActionResult> Handle(
        ExecuteCombatActionCommand command,
        CancellationToken cancellationToken = default)
    {
        var combatContext =
            await _combatContextRepository.GetByIdAsync(
                command.CombatContextId,
                cancellationToken);

        if (combatContext is null)
        {
            throw new InvalidOperationException(
                "Combat context not found.");
        }

        if (combatContext.State != CombatState.Active)
        {
            throw new InvalidOperationException(
                "Only active combat contexts accept actions.");
        }

        var action =
            new CombatAction(
                command.ActionId);

        return combatContext.ProcessAction(action);
    }
}
