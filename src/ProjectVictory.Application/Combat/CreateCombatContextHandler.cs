using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Combat;

public sealed class CreateCombatContextHandler
{
    private readonly IHuntRepository _huntRepository;
    private readonly ICombatContextRepository _combatContextRepository;

    public CreateCombatContextHandler(
        IHuntRepository huntRepository,
        ICombatContextRepository combatContextRepository)
    {
        _huntRepository = huntRepository;
        _combatContextRepository = combatContextRepository;
    }

    public async Task<CreateCombatContextResult> Handle(
        CreateCombatContextCommand command,
        CancellationToken cancellationToken = default)
    {
        var hunt = await _huntRepository.GetByIdAsync(
            command.HuntId,
            cancellationToken);

        if (hunt is null)
        {
            throw new InvalidOperationException(
                "Hunt not found.");
        }

        if (hunt.State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Only running hunts can start combat.");
        }

        var combatContext = new CombatContext(
            Guid.NewGuid(),
            hunt.Id);

        combatContext.Activate();

        await _combatContextRepository.AddAsync(
            combatContext,
            cancellationToken);

        return new CreateCombatContextResult(
            combatContext.Id,
            combatContext.HuntId);
    }
}