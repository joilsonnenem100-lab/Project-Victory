using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Combat;

public sealed class FinalizeCombatHandler
{
    private readonly ICombatContextRepository _combatContextRepository;

    public FinalizeCombatHandler(
        ICombatContextRepository combatContextRepository)
    {
        _combatContextRepository = combatContextRepository;
    }

    public async Task<FinalizeCombatResult> Handle(
        FinalizeCombatCommand command,
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

        combatContext.FinalizeCombat();

        return new FinalizeCombatResult(
            combatContext.Id,
            true);
    }
}