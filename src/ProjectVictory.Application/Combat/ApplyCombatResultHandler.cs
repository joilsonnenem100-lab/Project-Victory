using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Combat;

public sealed class ApplyCombatResultHandler
{
    private readonly ICombatContextRepository _combatContextRepository;

    public ApplyCombatResultHandler(
        ICombatContextRepository combatContextRepository)
    {
        _combatContextRepository = combatContextRepository;
    }

    public async Task<ApplyCombatResultResult> Handle(
        ApplyCombatResultCommand command,
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
                "Only active combat contexts can receive results.");
        }

        if (command.ResolutionId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Combat resolution is required.");
        }

        combatContext.ApplyResolution();

        return new ApplyCombatResultResult(
            combatContext.Id,
            true);
    }
}