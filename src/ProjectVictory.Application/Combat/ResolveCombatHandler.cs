using System;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Combat;

public sealed class ResolveCombatHandler
{
    private readonly ICombatContextRepository _combatContextRepository;
    private readonly ICombatResolver _combatResolver;

    public ResolveCombatHandler(
        ICombatContextRepository combatContextRepository,
        ICombatResolver combatResolver)
    {
        _combatContextRepository = combatContextRepository;
        _combatResolver = combatResolver;
    }

    public async Task<ResolveCombatResult> Handle(
        ResolveCombatCommand command,
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
                "Only active combat contexts can be resolved.");
        }

        var result =
            _combatResolver.Resolve(
                new CombatRequest(
                    combatContext.Id,
                    combatContext.AttackerId,
                    combatContext.MonsterId));

        combatContext.ApplyResolution();

        return new ResolveCombatResult(
            combatContext.Id,
            result.WinnerId,
            result.Completed);
    }
}
