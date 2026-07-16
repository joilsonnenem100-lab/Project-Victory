using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Combat;

public sealed class GenerateCombatActionResultHandler
{
    private readonly ICombatContextRepository _combatContextRepository;

    public GenerateCombatActionResultHandler(
        ICombatContextRepository combatContextRepository)
    {
        _combatContextRepository = combatContextRepository;
    }

    public async Task<CombatActionExecutionResult> Handle(
        GenerateCombatActionResultCommand command,
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

        if (combatContext.State != CombatState.Active)
        {
            throw new InvalidOperationException(
                "Only active combat contexts can generate results.");
        }

        var action = new CombatAction(
            command.ActionId);

        var processed = combatContext.ProcessAction(
            action);

        if (!processed.Processed)
        {
            throw new InvalidOperationException(
                "Combat action was not processed.");
        }

        return new CombatActionExecutionResult(
            Guid.NewGuid(),
            combatContext.Id);
    }
}