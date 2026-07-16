using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Combat;

public sealed class GenerateCombatResolutionHandler
{
    private readonly ICombatContextRepository _combatContextRepository;

    public GenerateCombatResolutionHandler(
        ICombatContextRepository combatContextRepository)
    {
        _combatContextRepository = combatContextRepository;
    }

    public async Task<GenerateCombatResolutionResult> Handle(
        GenerateCombatResolutionCommand command,
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
                "Only active combat contexts can generate resolutions.");
        }

        if (command.ActionResultId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Action result is required.");
        }

        var resolution = new CombatResolution(
            Guid.NewGuid(),
            combatContext.Id,
            command.ActionResultId);

        return new GenerateCombatResolutionResult(
            resolution.Id,
            resolution.CombatContextId);
    }
}