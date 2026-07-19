using ProjectVictory.Application.Combat;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ExecuteExistingHuntHandler
{
    private readonly IHuntRepository _huntRepository;
    private readonly ICombatResolver _combatResolver;

    public ExecuteExistingHuntHandler(
        IHuntRepository huntRepository,
        ICombatResolver combatResolver)
    {
        _huntRepository = huntRepository;
        _combatResolver = combatResolver;
    }

    public async Task<ExecuteHuntResult> Handle(
        ExecuteExistingHuntCommand command,
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

        hunt.Start();

        var result = _combatResolver.Resolve(
            new CombatRequest(
                Guid.NewGuid(),
                hunt.CharacterId,
                command.EnemyId));

        if (!result.Completed)
        {
            hunt.Fail();

            return new ExecuteHuntResult(
                hunt.Id,
                false,
                0,
                0,
                0);
        }

        hunt.Complete();

        return new ExecuteHuntResult(
            hunt.Id,
            true,
            0,
            0,
            0);
    }
}